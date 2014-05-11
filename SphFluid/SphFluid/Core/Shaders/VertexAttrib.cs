using log4net;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    public class VertexAttrib
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VertexAttrib));

        public int Index { get; private set; }
        public int Components { get; private set; }
        public VertexAttribPointerType Type { get; private set; }

        public VertexAttrib(int program, string name, int components, VertexAttribPointerType type)
        {
            Index = GL.GetAttribLocation(program, name);
            if (Index == -1) Logger.WarnFormat("Vertex attribute not found or not active: {0}", name);
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