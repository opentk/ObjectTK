using OpenTK.Mathematics;

namespace ObjectTK._2D {
    public class Camera2D {
        public Transform2D Transform { get; } = new Transform2D();

        /// The near clipping plane for this camera.
        public int NearClip { get; set; } = 1;

        /// The far clipping plane for this camera.
        public int FarClip { get; set; } = 100;


        /// The number of units covered in the vertical of this camera.
        public float VerticalSize { get; set; } = 10;

        /// Given by viewport/screen width/height<br/>
        /// The aspect ratio of the viewport. This should match the window or viewport's aspect ratio.
        /// This is used to derive the horizontal size from the vertical height.
        /// <see cref="VerticalSize" />
        public float AspectRatio { get; set; } = 1;

        public Vector2 Position {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        public float Rotation {
            get => Transform.Rotation;
            set => Transform.Rotation = value;
        }

        public float HorizontalSize => VerticalSize * AspectRatio;

        public Matrix4 Projection => Matrix4.CreateOrthographic(HorizontalSize, VerticalSize, NearClip, FarClip);

        public Matrix4 View => Matrix4.CreateRotationZ(Rotation) * Matrix4.CreateTranslation(Position.X, Position.Y, -10);

        public Matrix4 ViewProjection => View * Projection;
    }
}
