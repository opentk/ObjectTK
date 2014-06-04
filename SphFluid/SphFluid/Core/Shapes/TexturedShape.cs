using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class TexturedShape
        : Shape
    {
        public Vector2[] TexCoords { get; protected set; }
        public Buffer<Vector2> TexCoordBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            TexCoordBuffer = new Buffer<Vector2>();
            TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            if (TexCoordBuffer != null) TexCoordBuffer.Release();
        }

        public override void RenderImmediate(PrimitiveType mode)
        {
            throw new NotImplementedException();
        }
    }
}