using DerpGL.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

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

        public override void Dispose()
        {
            base.Dispose();
            if (TexCoordBuffer != null) TexCoordBuffer.Dispose();
        }
    }
}