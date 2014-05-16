using log4net;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    public class VertexAttrib
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VertexAttrib));

        public readonly string Name;
        public readonly int Index;
        protected readonly VertexAttribAttribute Parameters;

        public VertexAttrib(int program, string name, VertexAttribAttribute parameters)
        {
            Name = name;
            Parameters = parameters;
            Index = GL.GetAttribLocation(program, name);
            if (Index == -1) Logger.WarnFormat("Vertex attribute not found or not active: {0}", name);
        }

        public void Bind<T>(Vbo<T> buffer)
            where T : struct
        {
            Bind(buffer, 0, 0, Parameters.Normalized);
        }

        public void Bind<T>(Vbo<T> buffer, int stride, int offset, bool normalized)
            where T : struct
        {
            if (Index == -1) return;
            // bind given buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            // make sure the vertex attribute is enabled
            GL.EnableVertexAttribArray(Index);
            // set the vertex attribute pointer to the current buffer
            GL.VertexAttribPointer(Index, Parameters.Components, Parameters.Type, normalized, stride, offset);
            //TODO: VertexAttribArrays currently never get disabled again, maybe improve performance by doing so
        }
    }
}