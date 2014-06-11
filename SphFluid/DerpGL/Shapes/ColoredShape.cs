using System;
using DerpGL.Buffers;
using OpenTK.Graphics.OpenGL;

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

        protected override void OnRelease()
        {
            base.OnRelease();
            if (ColorBuffer != null) ColorBuffer.Release();
        }

        public override void RenderImmediate(PrimitiveType mode)
        {
            throw new NotImplementedException();
        }
    }
}