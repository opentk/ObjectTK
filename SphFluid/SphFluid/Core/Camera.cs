using System;
using System.Collections.Generic;
using log4net;
using OpenTK;
using OpenTK.Input;

namespace SphFluid.Core
{
    public class Camera
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Camera));

        public Vector3 Position;
        public float Yaw = 0;
        public float Pitch = 0;
        public float Roll;

        public Vector3 DefaultPosition = new Vector3(0, 0, 2.2f);
        public float DefaultYaw = 55;
        public float DefaultPitch = 17;
        public float DefaultRoll = 0;

        private readonly GameWindow _window;
        private readonly List<MouseButton> _mouseButtonStates;
        private Vector2 _mousePosition;
        
        private const float MoveSpeed = 0.5f;
        private const float MouseSpeed = 0.5f;

        public Camera(GameWindow window)
        {
            Logger.Info("Initializing camera");
            _window = window;
            _mouseButtonStates = new List<MouseButton>();
            RegisterEvents();
            ResetToDefault();
        }

        public void RegisterEvents()
        {
            _window.Mouse.ButtonDown += MouseButtonDown;
            _window.Mouse.Move += MouseMove;
            _window.Mouse.ButtonUp += MouseButtonUp;
            _window.Mouse.WheelChanged += MouseWheelChanged;
            _window.Keyboard.KeyDown += KeyboardKeyDown;
        }

        public void UnregisterEvents()
        {
            _window.Mouse.ButtonDown -= MouseButtonDown;
            _window.Mouse.Move -= MouseMove;
            _window.Mouse.ButtonUp -= MouseButtonUp;
            _window.Mouse.WheelChanged -= MouseWheelChanged;
            _window.Keyboard.KeyDown -= KeyboardKeyDown;
        }

        public void ApplyCamera(ref Matrix4 modelView)
        {
            modelView = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Yaw))
                * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Roll))
                * Matrix4.CreateTranslation(-Position)
                * modelView;
        }

        public void ApplyRotation(ref Vector3 vector)
        {
            var rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-Pitch))
                * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-Yaw));
            Vector3.Transform(ref vector, ref rotation, out vector);
        }

        public Vector3 GetEyePosition(ref Matrix4 modelViewMatrix)
        {
            var modelView = modelViewMatrix;
            modelView.Invert();
            var position = new Vector4(Position, 0);
            Vector4.Transform(ref position, ref modelView, out position);
            return position.Xyz;
        }

        public Vector3 GetPickingRay(int mouseX, int mouseY)
        {
            const float fieldOfView = MathHelper.PiOver4;
            const float nearClippingPaneDistance = 1;
            var view = -Position;
            view.Normalize();
            var cameraUp = Vector3.UnitY;
            var h = Vector3.Cross(view, cameraUp);
            h.Normalize();
            var v = Vector3.Cross(h, view);
            v.Normalize();
            var vLength = (float)Math.Tan(fieldOfView / 2) * nearClippingPaneDistance;
            var hLength = vLength * ((float)_window.Width / _window.Height);
            v = v * vLength;
            h = h * hLength;
            //Vector3.Multiply(ref v, vLength, out v);
            //Vector3.Multiply(ref h, hLength, out h);
            // scale mouse position to [-1,1]
            var x  = 2f * mouseX / _window.Width - 1;
            var y = 1 - 2f * mouseY / _window.Height;
            var pos = Position + view * nearClippingPaneDistance + h * x + v * y;
            var dir = pos - Position;
            return dir;
        }

        private void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mousePosition = new Vector2(e.Position.X, e.Position.Y);
            if (!_mouseButtonStates.Contains(e.Button))
            {
                _mouseButtonStates.Add(e.Button);
            }
        }

        private void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseButtonStates.Contains(e.Button))
            {
                _mouseButtonStates.Remove(e.Button);
            }
        }

        private void MouseMove(object sender, MouseMoveEventArgs e)
        {
            if (_mouseButtonStates.Count != 1) return;
            var dx = -e.XDelta;
            var dy = -e.YDelta;
            switch (_mouseButtonStates[0])
            {
                case MouseButton.Right:
                    Yaw -= dx * MouseSpeed;
                    Pitch -= dy * MouseSpeed;
                    break;
                case MouseButton.Middle:
                    if (dy > 100) dy = 100;
                    Position.Z *= 1 + dy * MouseSpeed / 100;
                    break;
            }
            // remember mouse position
            _mousePosition.X = e.Position.X;
            _mousePosition.Y = e.Position.Y;
        }

        private void MouseWheelChanged(object sender, MouseWheelEventArgs e)
        {
            var dy = -e.DeltaPrecise;
            if (dy > 100) dy = 100;
            Position.Z *= 1 + dy * MouseSpeed / 10;
        }

        private void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    ResetToDefault();
                    break;
            }
        }

        public void ResetToDefault()
        {
            Position = DefaultPosition;
            Yaw = DefaultYaw;
            Pitch = DefaultPitch;
            Roll = DefaultRoll;
        }
    }
}