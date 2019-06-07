//
// FreeLookBehavior.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

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