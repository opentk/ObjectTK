using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public class TexturedQuad
        : TexturedShape
    {
        public TexturedQuad()
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
            
            TexCoords = new[]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(0,1),
                new Vector2(1,1)
            };
        }
    }
}