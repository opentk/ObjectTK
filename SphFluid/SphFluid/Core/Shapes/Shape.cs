using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class Shape
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }
        public Vbo<Vector3> VertexBuffer { get; protected set; }

        public virtual void UpdateBuffers()
        {
            VertexBuffer = new Vbo<Vector3>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
        }

        public void RenderImmediate()
        {
            RenderImmediate(DefaultMode);
        }

        public virtual void RenderImmediate(PrimitiveType mode)
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
