using OpenTK;
using OpenTK.Input;

namespace DerpGL.Cameras
{
    /// <summary>
    /// Camera which is centered on and rotates around an origin.<br/>
    /// The cameras position is relative to the origin, use <see cref="GetEyePosition"/> to get the position in world coordinates.
    /// </summary>
    public class RotateAroundOriginCamera
        : CameraBase
    {
        /// <summary>
        /// Specifies the origin around which the camera rotates.
        /// </summary>
        public Vector3 Origin;

        /// <summary>
        /// Specifies the default origin which is used when <see cref="ResetToDefault"/> is called.
        /// </summary>
        public Vector3 DefaultOrigin;

        /// <summary>
        /// Specifies the mouse speed used when rotating.
        /// </summary>
        public float MouseSpeed = 0.005f;

        /// <summary>
        /// Specifies the speed of the mouse wheel when zooming.
        /// </summary>
        public float WheelSpeed = 0.1f;

        /// <summary>
        /// Initializes a new instance of RotateAroundOriginCamera. 
        /// </summary>
        public RotateAroundOriginCamera()
        {
            // prevent the position from starting at zero because the zoom mechanism below then fail
            DefaultPosition.Z = 1;
            Position.Z = 1;
        }

        public override void ResetToDefault()
        {
            base.ResetToDefault();
            Origin = DefaultOrigin;
        }

        public override void ApplyCamera(ref Matrix4 matrix)
        {
            matrix = Matrix4.CreateTranslation(-Origin)
                     * Matrix4.CreateRotationY(Yaw)
                     * Matrix4.CreateRotationX(Pitch)
                     * Matrix4.CreateRotationZ(Roll)
                     * Matrix4.CreateTranslation(-Position)
                     * matrix;
        }

        public override Vector3 GetEyePosition()
        {
            var transformation = Matrix4.Identity;
            ApplyCamera(ref transformation);
            var position = Vector4.UnitW;
            Vector4.Transform(ref position, ref transformation, out position);
            return -position.Xyz;
        }

        public override void Enable(GameWindow window)
        {
            window.Mouse.Move += MouseMove;
            window.Mouse.WheelChanged += MouseWheelChanged;
        }

        public override void Disable(GameWindow window)
        {
            window.Mouse.Move -= MouseMove;
            window.Mouse.WheelChanged -= MouseWheelChanged;
        }

        private void MouseMove(object sender, MouseMoveEventArgs e)
        {
            var dx = -e.XDelta;
            var dy = -e.YDelta;
            var state = Mouse.GetState();
            if (state.IsButtonDown(MouseButton.Left))
            {
                Yaw -= dx*MouseSpeed;
                Pitch -= dy*MouseSpeed;
            }
            if (state.IsButtonDown(MouseButton.Right))
            {
                if (dy > 100) dy = 100;
                Position.Z *= 1 + dy * MouseSpeed;
            }
        }

        private void MouseWheelChanged(object sender, MouseWheelEventArgs e)
        {
            var dy = -e.DeltaPrecise;
            if (dy > 100) dy = 100;
            Position.Z *= 1 + dy * WheelSpeed;
        }
    }
}