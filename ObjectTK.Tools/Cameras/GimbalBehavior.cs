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
    /// <summary>
    /// TODO: Does not yet work like it should.
    /// </summary>
    public class GimbalBehavior
        : ThirdPersonBehavior
    {
        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                base.MouseMove(state, delta);
                var leftRight = Vector3.Cross(state.Up, state.LookAt);
                Vector3.Cross(ref state.LookAt, ref leftRight, out state.Up);
                state.Up.Normalize();
            }
        }
    }
}