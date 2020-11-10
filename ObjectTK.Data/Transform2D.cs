using OpenTK.Mathematics;

namespace ObjectTK.Data {
    public class Transform2D {
        
        /// The position of this object on the X/Y plane.
        public Vector2 Position { get; set; }
        /// [DEGREES]. The orientation of this object, clockwise about the Z Axis (through the screen).<br/>
        /// 0 degrees = pointing up to the top of the screen.<br/>
        /// 90 degrees = pointing to the right of the screen.<br/>
        /// etc.
        public float Rotation { get; set; }
    }
}
