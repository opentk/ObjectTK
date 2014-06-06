using DerpGL.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class VertexAttrib
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VertexAttrib));

        public readonly int Index;
        protected readonly VertexAttribAttribute Parameters;

        public VertexAttrib(int program, string name, VertexAttribAttribute parameters)
            : base(program, name)
        {
            Parameters = parameters;
            Index = GL.GetAttribLocation(program, name);
            if (Index == -1) Logger.WarnFormat("Vertex attribute not found or not active: {0}", name);
        }

        //TODO: take the stride from the buffer.ElementSize?
        public void Bind<T>(Buffer<T> buffer)
            where T : struct
        {
            Bind(buffer, Parameters.Components, Parameters.Type, 0, 0, Parameters.Normalized);
        }

        public void Bind<T>(Buffer<T> buffer, int stride, int offset)
            where T : struct
        {
            Bind(buffer, Parameters.Components, Parameters.Type, stride, offset, Parameters.Normalized);
        }

        public void Bind<T>(Buffer<T> buffer, int stride, int offset, bool normalized)
            where T : struct
        {
            Bind(buffer, Parameters.Components, Parameters.Type, stride, offset, normalized);
        }

        public void Bind<T>(Buffer<T> buffer, int components, int stride, int offset)
            where T : struct
        {
            Bind(buffer, components, Parameters.Type, stride, offset, Parameters.Normalized);
        }

        public void Bind<T>(Buffer<T> buffer, int components, int stride, int offset, bool normalized)
            where T : struct
        {
            Bind(buffer, components, Parameters.Type, stride, offset, normalized);
        }

        public void Bind<T>(Buffer<T> buffer, int components, VertexAttribPointerType type, int stride, int offset, bool normalized)
            where T : struct
        {
            if (Index == -1) return;
            // bind given buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            // make sure the vertex attribute is enabled
            GL.EnableVertexAttribArray(Index);
            // set the vertex attribute pointer to the current buffer
            GL.VertexAttribPointer(Index, components, type, normalized, stride, offset);
            //TODO: VertexAttribArrays currently never get disabled again, maybe improve performance by doing so
        }
    }
}