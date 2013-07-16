using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class IndexedShape
        : Shape
    {
        public int[] Indices { get; protected set; }

        public override Vao CreateVao(BeginMode mode)
        {
            return new IndexedShapeVao(this, mode);
        }

        public override void Render(BeginMode mode)
        {
            GL.Begin(mode);
            foreach (var index in Indices)
            {
                GL.Vertex3(Vertices[index]);
            }
            GL.End();
        }
    }
}