using DerpGL.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public abstract class Shape
        : ContextResource
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }
        public Buffer<Vector3> VertexBuffer { get; protected set; }

        protected Shape()
        {
            ReleaseRequired = false;
        }

        public virtual void UpdateBuffers()
        {
            ReleaseRequired = true;
            VertexBuffer = new Buffer<Vector3>();
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
