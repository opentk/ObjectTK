using System;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Shapes;

namespace SphFluid.Core.Buffers
{
    public class ColoredShapeVao
        : IndexedShapeVao
    {
        private readonly Vbo<int> _colorBuffer;

        protected ColoredShapeVao(ColoredShape shape, BeginMode mode, int drawCount)
            : base(shape, mode, drawCount)
        {
            GL.BindVertexArray(VaoHandle);
            // create color buffer
            GL.EnableClientState(ArrayCap.ColorArray);
            _colorBuffer = new Vbo<int>();
            _colorBuffer.UploadData(BufferTarget.ArrayBuffer, shape.Colors);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, IntPtr.Zero);
            // unbind vertex array object
            GL.BindVertexArray(0);
        }

        public ColoredShapeVao(ColoredShape shape, BeginMode mode)
            : this(shape, mode, shape.Indices.Length) { }

        public override void Release()
        {
            base.Release();
            _colorBuffer.Release();
        }
    }
}