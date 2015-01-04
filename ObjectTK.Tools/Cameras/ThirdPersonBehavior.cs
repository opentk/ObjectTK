#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using OpenTK;
using OpenTK.Input;

namespace ObjectTK.Tools.Cameras
{
    public class ThirdPersonBehavior
        : CameraBehavior
    {
        public Vector3 Origin;

        protected void UpdateDistance(CameraState state, float scale)
        {
            state.Position = Origin - (state.Position - Origin).Length * (1 + scale) * state.LookAt;
        }

        public override void Initialize(CameraState state)
        {
            // recalculate look at direction
            state.LookAt = Origin - state.Position;
            state.LookAt.Normalize();
        }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                // rotate look direction with mouse
                HandleFreeLook(state, delta);
                // recalculate the position
                UpdateDistance(state, 0);
            }
            if (mouse.IsButtonDown(MouseButton.Right))
            {
                UpdateDistance(state, delta.Y);
            }
        }

        public override void MouseWheelChanged(CameraState state, float delta)
        {
            if (delta > 100) delta = 100;
            UpdateDistance(state, -delta);
        }
    }
}