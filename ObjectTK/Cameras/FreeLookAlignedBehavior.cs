using OpenTK;

namespace ObjectTK.Cameras
{
    public class FreeLookAlignedBehavior
        : FreeLookBehavior
    {
        public Vector3 AlignmentPoint;

        public override void UpdateFrame(CameraState state, float step)
        {
            base.UpdateFrame(state, step);
            // update the up direction to always point away from the alignment point
            var up = state.Position - AlignmentPoint;
            up.Normalize();
            // if the angle between the old and new up directions is larger than 90°
            // we assume that the camera is upside down and keep it that way
            state.Up = Vector3.Dot(state.Up, up) < 0 ? -up : up;
        }
    }
}