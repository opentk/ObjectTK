using OpenTK;

namespace ObjectTK.Cameras
{
    /// <summary>
    /// Represents the state of a camera.<br/>
    /// TODO: Maybe also add field-of-view and z-near/far plane parameter
    /// </summary>
    public class CameraState
    {
        public Vector3 Position;
        public Vector3 LookAt;
        public Vector3 Up;

        public CameraState()
        {
            LookAt.Z = -1;
            Up.Y = 1;
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", Position, LookAt, Up);
        }
    }
}