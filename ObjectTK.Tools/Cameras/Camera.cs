//
// Camera.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK;
using OpenTK.Input;

namespace ObjectTK.Tools.Cameras
{
    public class Camera
    {
        public CameraState State;
        public CameraState DefaultState;
        protected CameraBehavior Behavior;

        public float MouseMoveSpeed = 0.005f;
        public float MouseWheelSpeed = 0.1f;
        public float MoveSpeed = 60;

        public Camera()
        {
            State = new CameraState();
            DefaultState = new CameraState();
        }

        public void ResetToDefault()
        {
            State.Position = DefaultState.Position;
            State.LookAt = DefaultState.LookAt;
            State.Up = DefaultState.Up;
            Update();
        }

        public void SetBehavior(CameraBehavior behavior)
        {
            Behavior = behavior;
            Update();
        }

        public void Enable(GameWindow window)
        {
            if (Behavior == null) throw new InvalidOperationException("Can not enable Camera while the Behavior is not set.");
            window.UpdateFrame += UpdateFrame;
            window.Mouse.Move += MouseMove;
            window.Mouse.WheelChanged += MouseWheelChanged;
        }

        public void Disable(GameWindow window)
        {
            window.UpdateFrame -= UpdateFrame;
            window.Mouse.Move -= MouseMove;
            window.Mouse.WheelChanged -= MouseWheelChanged;
        }

        public void Update()
        {
            if (Behavior != null) Behavior.Initialize(State);
        }

        private void UpdateFrame(object sender, FrameEventArgs e)
        {
            Behavior.UpdateFrame(State, (float) e.Time * MoveSpeed);
        }

        private void MouseMove(object sender, MouseMoveEventArgs e)
        {
            Behavior.MouseMove(State, MouseMoveSpeed * new Vector2(e.XDelta, e.YDelta));
        }
        
        private void MouseWheelChanged(object sender, MouseWheelEventArgs e)
        {
            Behavior.MouseWheelChanged(State, MouseWheelSpeed * e.DeltaPrecise);
        }

        /// <summary>
        /// TODO: add smooth transitions for the CameraState variables
        /// </summary>
        public Matrix4 GetCameraTransform()
        {
            // kind of hack: prevent look-at and up directions to be parallel
            if (Math.Abs(Vector3.Dot(State.Up, State.LookAt)) > 0.99999999999) State.LookAt += 0.001f * new Vector3(3,5,4);
            return Matrix4.LookAt(State.Position, State.Position + State.LookAt, State.Up);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", State, Behavior);
        }
    }
}

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