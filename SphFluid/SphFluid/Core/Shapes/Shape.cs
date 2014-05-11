using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class Shape
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }

        public Vao CreateVao()
        {
            return CreateVao(DefaultMode);
        }

        public virtual Vao CreateVao(PrimitiveType mode)
        {
            return new ShapeVao(this, mode);
        }

        public virtual void Render()
        {
            Render(DefaultMode);
        }

        public virtual void Render(PrimitiveType mode)
        {
            GL.Begin(mode);
            foreach (var vertex in Vertices)
            {
                GL.Vertex3(vertex);
            }
            GL.End();
        }
    }
}
