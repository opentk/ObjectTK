using System;
using OpenTK;
using OpenTK.Input;

namespace DerpGL.Cameras
{
    /// <summary>
    /// Camera which can move freely but is constrained in its orientation to mimic the perspective of a person:<br/>
    /// - Roll angle is ignored<br/>
    /// - Pitch angle is constrained to "straight up" and "straight down", i.e. abs(Pitch) &#8804; PI/2
    /// </summary>
    public class FirstPersonCamera
        : CameraBase
    {
        /// <summary>
        /// Specifies the mouse speed when rotating.
        /// </summary>
        public float MouseSpeed = 0.0025f;

        /// <summary>
        /// Specifies the speed when moving.
        /// </summary>
        public float MoveSpeed = 6f;

        public override void ApplyCamera(ref Matrix4 matrix)
        {
            matrix = Matrix4.CreateTranslation(-Position)
                     * Matrix4.CreateRotationY(Yaw)
                     * Matrix4.CreateRotationX(Pitch)
                     * matrix;
        }

        public override Vector3 GetEyePosition()
        {
            return Position;
        }

        public override void Enable(GameWindow window)
        {
            window.Mouse.Move += MouseMove;
            window.UpdateFrame += UpdateFrame;
        }

        public override void Disable(GameWindow window)
        {
            window.Mouse.Move -= MouseMove;
            window.UpdateFrame -= UpdateFrame;
        }

        private void MouseMove(object sender, MouseMoveEventArgs e)
        {
            var dx = -e.XDelta;
            var dy = -e.YDelta;
            var state = Mouse.GetState();
            if (state.IsButtonDown(MouseButton.Left))
            {
                Yaw -= dx * MouseSpeed;
                Pitch -= dy * MouseSpeed;
            }
            if (Math.Abs(Pitch) > MathHelper.PiOver2) Pitch = Math.Sign(Pitch) * MathHelper.PiOver2;
        }

        private void UpdateFrame(object sender, FrameEventArgs e)
        {
            var speed = (float) (MoveSpeed * e.Time);
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Key.W)) Position -= new Vector3(MathF.Sin(-Yaw) * MathF.Cos(Pitch), MathF.Sin(Pitch), MathF.Cos(Yaw) * MathF.Cos(Pitch)) * speed;
            if (state.IsKeyDown(Key.S)) Position += new Vector3(MathF.Sin(-Yaw) * MathF.Cos(Pitch), MathF.Sin(Pitch), MathF.Cos(Yaw) * MathF.Cos(Pitch)) * speed;
            if (state.IsKeyDown(Key.A)) Position -= new Vector3(MathF.Sin(-Yaw + MathHelper.PiOver2), 0, MathF.Cos(Yaw - MathHelper.PiOver2)) * speed;
            if (state.IsKeyDown(Key.D)) Position += new Vector3(MathF.Sin(-Yaw + MathHelper.PiOver2), 0, MathF.Cos(Yaw - MathHelper.PiOver2)) * speed;
            if (state.IsKeyDown(Key.Space)) Position += new Vector3(0, 1, 0) * speed;
            if (state.IsKeyDown(Key.LControl)) Position -= new Vector3(0, 1, 0) * speed;
        }
    }
}