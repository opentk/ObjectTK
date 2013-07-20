using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    public class VertexAttrib
    {
        public int Index { get; private set; }
        public int Components { get; private set; }
        public VertexAttribPointerType Type { get; private set; }

        public VertexAttrib(int index, int components, VertexAttribPointerType type)
        {
            Index = index;
            Components = components;
            Type = type;
        }

        public void Bind<T>(Vbo<T> buffer)
            where T : struct
        {
            Bind(buffer, 0, 0, false);
        }

        public void Bind<T>(Vbo<T> buffer, int stride, int offset, bool normalized)
            where T : struct
        {
            // bind given buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            // make sure the vertex attribute is enabled
            GL.EnableVertexAttribArray(Index);
            // set the vertex attribute pointer to the current buffer
            GL.VertexAttribPointer(Index, Components, Type, normalized, stride, offset);
        }
    }
}