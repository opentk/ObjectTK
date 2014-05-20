using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class ColoredShape
        : IndexedShape
    {
        public uint[] Colors { get; protected set; }
        public Vbo<uint> ColorBuffer { get; protected set; }

        public static uint ColorToRgba32(Color c)
        {
            return (uint) ((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
        }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            ColorBuffer = new Vbo<uint>();
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