using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class IndexedShape
        : Shape
    {
        //TODO: use unsigned ints?
        public int[] Indices { get; protected set; }
        public Vbo<int> IndexBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            IndexBuffer = new Vbo<int>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices);
        }

        public override void RenderImmediate(PrimitiveType mode)
        {
            GL.Begin(mode);
            foreach (var index in Indices)
            {
                GL.Vertex3(Vertices[index]);
            }
            GL.End();
        }
    }
}