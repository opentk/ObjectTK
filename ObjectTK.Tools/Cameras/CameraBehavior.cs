//
// CameraBehavior.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK;

namespace ObjectTK.Tools.Cameras
{
    public abstract class CameraBehavior
    {
        public virtual void Initialize(CameraState state) { }
        public virtual void UpdateFrame(CameraState state, float step) { }
        public virtual void MouseMove(CameraState state, Vector2 delta) { }
        public virtual void MouseWheelChanged(CameraState state, float delta) { }

        /// <summary>
        /// TODO: add possibility to limit the pitch and prevent "flipping over"
        /// </summary>
        protected void HandleFreeLook(CameraState state, Vector2 delta)
        {
            var leftRight = Vector3.Cross(state.Up, state.LookAt);
            var forward = Vector3.Cross(leftRight, state.Up);
            // rotate look at direction
            var rot = Matrix4.CreateFromAxisAngle(state.Up, -delta.X) * Matrix4.CreateFromAxisAngle(leftRight, delta.Y);
            Vector3.Transform(ref state.LookAt, ref rot, out state.LookAt);
            // renormalize to prevent summing up of floating point errors
            state.LookAt.Normalize();
            // flip up vector when pitched more than +/-90� from the forward direction
            if (Vector3.Dot(state.LookAt, forward) < 0) state.Up *= -1;
        }
    }
}