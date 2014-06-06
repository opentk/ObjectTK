using System;
using DerpGL.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
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