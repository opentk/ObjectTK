using OpenTK;
using OpenTK.Input;

namespace ObjectTK.Cameras
{
    public class ThirdPersonBehavior
        : CameraBehavior
    {
        public Vector3 Origin;

        protected void UpdateDistance(CameraState state, float scale)
        {
            state.Position = Origin - (state.Position - Origin).Length * (1 + scale) * state.LookAt;
        }

        public override void Initialize(CameraState state)
        {
            // recalculate look at direction
            state.LookAt = Origin - state.Position;
            state.LookAt.Normalize();
        }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                // rotate look direction with mouse
                HandleFreeLook(state, delta);
                // recalculate the position
                UpdateDistance(state, 0);
            }
            if (mouse.IsButtonDown(MouseButton.Right))
            {
                UpdateDistance(state, delta.Y);
            }
        }

        public override void MouseWheelChanged(CameraState state, float delta)
        {
            if (delta > 100) delta = 100;
            UpdateDistance(state, -delta);
        }
    }
}