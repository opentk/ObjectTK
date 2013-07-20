using System;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Shapes;

namespace SphFluid.Core.Buffers
{
    public class IndexedShapeVao
        : ShapeVao
    {
        private readonly Vbo<int> _elementBuffer;

        protected IndexedShapeVao(IndexedShape shape, BeginMode mode, int drawCount)
            : base(shape, mode, drawCount)
        {
            GL.BindVertexArray(VaoHandle);
            // create index buffer (elements inside the vertex buffer, not color indices as per the IndexPointer function!)
            _elementBuffer = new Vbo<int>();
            _elementBuffer.UploadData(BufferTarget.ElementArrayBuffer, shape.Indices);
            // unbind vertex array object
            GL.BindVertexArray(0);
        }

        public IndexedShapeVao(IndexedShape shape, BeginMode mode)
            : this(shape, mode, shape.Indices.Length) { }

        public override void Release()
        {
            base.Release();
            _elementBuffer.Release();
        }

        public override void Render()
        {
            GL.BindVertexArray(VaoHandle);
            GL.DrawElements(Mode, DrawCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }

        public override void RenderInstanced(int instances)
        {
            GL.BindVertexArray(VaoHandle);
            GL.DrawElementsInstanced(Mode, DrawCount, DrawElementsType.UnsignedInt, IntPtr.Zero, instances);
            GL.BindVertexArray(0);
        }
    }
}