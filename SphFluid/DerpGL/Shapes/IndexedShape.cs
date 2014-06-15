using DerpGL.Buffers;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public abstract class IndexedShape
        : Shape
    {
        public uint[] Indices { get; protected set; }
        public Buffer<uint> IndexBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            IndexBuffer = new Buffer<uint>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            if (IndexBuffer != null) IndexBuffer.Release();
        }

        public override void RenderImmediate(PrimitiveType mode)
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