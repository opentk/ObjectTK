using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class Shape
        : ContextResource
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }
        public Vbo<Vector3> VertexBuffer { get; protected set; }

        protected Shape()
        {
            ReleaseRequired = false;
        }

        public virtual void UpdateBuffers()
        {
            ReleaseRequired = true;
            VertexBuffer = new Vbo<Vector3>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
        }

        protected override void OnRelease()
        {
            if (VertexBuffer != null) VertexBuffer.Release();
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
