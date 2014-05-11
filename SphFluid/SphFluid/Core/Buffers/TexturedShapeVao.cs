using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Shapes;

namespace SphFluid.Core.Buffers
{
    public class TexturedShapeVao
        : ShapeVao
    {
        private readonly Vbo<Vector2> _texCoordBuffer;

        protected TexturedShapeVao(TexturedShape shape, PrimitiveType mode, int drawCount)
            : base(shape, mode, drawCount)
        {
            GL.BindVertexArray(VaoHandle);
            // create vertex buffer
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            _texCoordBuffer = new Vbo<Vector2>();
            _texCoordBuffer.Init(BufferTarget.ArrayBuffer, shape.TexCoords);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
            // unbind vertex array object
            GL.BindVertexArray(0);
        }

        public TexturedShapeVao(TexturedShape shape, PrimitiveType mode)
            : this(shape, mode, shape.Vertices.Length) { }

        public override void Release()
        {
            base.Release();
            _texCoordBuffer.Release();
        }
    }
}