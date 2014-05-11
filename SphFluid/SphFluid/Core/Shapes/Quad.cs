using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shapes
{
    public class Quad
        : Shape
    {
        public Quad()
        {
            DefaultMode = PrimitiveType.TriangleStrip;
            
            // Source: http://www.opengl.org/archives/resources/faq/technical/transformations.htm (9.090 How do I draw a full-screen quad?)
            // Your rectangle or quad's Z value should be in the range of –1.0 to 1.0, with –1.0 mapping to the zNear clipping plane, and 1.0 to the zFar clipping plane.
            const int z = 0;
            Vertices = new[]
            {
                new Vector3(-1, -1, z),
                new Vector3( 1, -1, z),
                new Vector3(-1,  1, z),
                new Vector3( 1,  1, z)
            };
        }
    }
}