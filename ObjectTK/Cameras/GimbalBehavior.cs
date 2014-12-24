using OpenTK;
using OpenTK.Input;

namespace ObjectTK.Cameras
{
    /// <summary>
    /// TODO: Does not yet work like it should.
    /// </summary>
    public class GimbalBehavior
        : ThirdPersonBehavior
    {
        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                base.MouseMove(state, delta);
                var leftRight = Vector3.Cross(state.Up, state.LookAt);
                Vector3.Cross(ref state.LookAt, ref leftRight, out state.Up);
                state.Up.Normalize();
            }
        }
    }
}