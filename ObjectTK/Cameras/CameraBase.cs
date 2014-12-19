#region License
// ObjectTK License
// Copyright (C) 2013-2014 J.C.Bernack
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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using OpenTK;

namespace ObjectTK.Cameras
{
    /// <summary>
    /// Base class for camera implementations.
    /// TODO: somehow seperate camera properties from the different implementations to enable switching between different cameras.
    /// </summary>
    public abstract class CameraBase
    {
        /// <summary>
        /// Specifies the position of the camera.<br/>
        /// This must not necessarily be the camera position in world coordinates, for that see <see cref="GetEyePosition"/>.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Specifies the yaw angle in radians.
        /// </summary>
        public float Yaw;

        /// <summary>
        /// Specifies the pitch angle in radians.
        /// </summary>
        public float Pitch;
        
        /// <summary>
        /// Specifies the roll angle in radians.
        /// </summary>
        public float Roll;

        /// <summary>
        /// Specifies the default position which is used when <see cref="ResetToDefault"/> is called.
        /// </summary>
        public Vector3 DefaultPosition;

        /// <summary>
        /// Specifies the default yaw angle which is used when <see cref="ResetToDefault"/> is called.
        /// </summary>
        public float DefaultYaw;

        /// <summary>
        /// Specifies the default pitch angle which is used when <see cref="ResetToDefault"/> is called.
        /// </summary>
        public float DefaultPitch;

        /// <summary>
        /// Specifies the default roll angle which is used when <see cref="ResetToDefault"/> is called.
        /// </summary>
        public float DefaultRoll;

        /// <summary>
        /// Resets the cameras position and orientation to the given default values.
        /// </summary>
        public virtual void ResetToDefault()
        {
            Position = DefaultPosition;
            Yaw = DefaultYaw;
            Pitch = DefaultPitch;
            Roll = DefaultRoll;
        }

        /// <summary>
        /// Applies the current view transformation to the given matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public abstract void ApplyCamera(ref Matrix4 matrix);

        /// <summary>
        /// Calculates the current camera position in world coordinates.
        /// </summary>
        /// <returns>The camera position in world coordinates</returns>
        public abstract Vector3 GetEyePosition();

        /// <summary>
        /// Enables the cameras controls, i.e. registers appropriate events.
        /// </summary>
        /// <param name="window">Specifies the window used to control the camera.</param>
        public abstract void Enable(GameWindow window);

        /// <summary>
        /// Disables the cameras controls, i.e. unregisters appropriate events.
        /// </summary>
        /// <param name="window">Specifies the window used to control the camera.</param>
        public abstract void Disable(GameWindow window);

        // for later use..
        //public Vector3 GetPickingRay(int mouseX, int mouseY)
        //{
        //    const float fieldOfView = MathHelper.PiOver4;
        //    const float nearClippingPaneDistance = 1;
        //    var view = -Position;
        //    view.Normalize();
        //    var cameraUp = Vector3.UnitY;
        //    var h = Vector3.Cross(view, cameraUp);
        //    h.Normalize();
        //    var v = Vector3.Cross(h, view);
        //    v.Normalize();
        //    var vLength = (float)Math.Tan(fieldOfView / 2) * nearClippingPaneDistance;
        //    var hLength = vLength * ((float)_window.Width / _window.Height);
        //    v = v * vLength;
        //    h = h * hLength;
        //    //Vector3.Multiply(ref v, vLength, out v);
        //    //Vector3.Multiply(ref h, hLength, out h);
        //    // scale mouse position to [-1,1]
        //    var x = 2f * mouseX / _window.Width - 1;
        //    var y = 1 - 2f * mouseY / _window.Height;
        //    var pos = Position + view * nearClippingPaneDistance + h * x + v * y;
        //    var dir = pos - Position;
        //    return dir;
        //}
    }
}