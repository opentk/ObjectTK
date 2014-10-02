using DerpGL.Buffers;
using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Shapes
{
    public abstract class ColoredShape
        : IndexedShape
    {
        public uint[] Colors { get; protected set; }
        public Buffer<uint> ColorBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            ColorBuffer = new Buffer<uint>();
            ColorBuffer.Init(BufferTarget.ArrayBuffer, Colors);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (ColorBuffer != null) ColorBuffer.Dispose();
        }
    }
}