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
            // flip up vector when pitched more than +/-90° from the forward direction
            if (Vector3.Dot(state.LookAt, forward) < 0) state.Up *= -1;
        }
    }
}