using System;
using System.Drawing;
using DerpGL.Core.Buffers;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Core.Shapes
{
    public abstract class ColoredShape
        : IndexedShape
    {
        public uint[] Colors { get; protected set; }
        public Buffer<uint> ColorBuffer { get; protected set; }

        public static uint ColorToRgba32(Color c)
        {
            return (uint) ((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
        }

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