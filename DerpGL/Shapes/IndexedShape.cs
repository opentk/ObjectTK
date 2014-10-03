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

        public override void Dispose()
        {
            base.Dispose();
            if (IndexBuffer != null) IndexBuffer.Dispose();
        }
    }
}