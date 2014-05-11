using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class TexturedShape
        : Shape
    {
        public Vector2[] TexCoords { get; protected set; }

        public override Vao CreateVao(PrimitiveType mode)
        {
            return new TexturedShapeVao(this, mode);
        }
    }
}