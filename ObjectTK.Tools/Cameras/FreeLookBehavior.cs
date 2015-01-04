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
    public class FreeLookBehavior
        : CameraBehavior
    {
        public override void UpdateFrame(CameraState state, float step)
        {
            var keyboard = Keyboard.GetState();
            var dir = Vector3.Zero;
            var leftRight = Vector3.Cross(state.Up, state.LookAt).Normalized();
            if (keyboard.IsKeyDown(Key.W)) dir += state.LookAt;
            if (keyboard.IsKeyDown(Key.S)) dir -= state.LookAt;
            if (keyboard.IsKeyDown(Key.A)) dir += leftRight;
            if (keyboard.IsKeyDown(Key.D)) dir -= leftRight;
            if (keyboard.IsKeyDown(Key.Space)) dir += state.Up;
            if (keyboard.IsKeyDown(Key.LControl)) dir -= state.Up;
            // normalize dir to enforce consistent movement speed, independent of the number of keys pressed
            if (dir.LengthSquared > 0) state.Position += dir.Normalized() * step;
        }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                HandleFreeLook(state, delta);
            }
        }
    }
}