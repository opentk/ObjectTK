using System.Collections.Generic;
using DerpGL.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a vertex attribute.
    /// </summary>
    public class VertexAttrib
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VertexAttrib));

        /// <summary>
        /// Holds the indices of all currently enabled vertex attributes.
        /// </summary>
        private static readonly List<int> EnabledVertexAttribArrays = new List<int>();

        /// <summary>
        /// Disable all vertex attributes.
        /// </summary>
        internal static void DisableVertexAttribArrays()
        {
            foreach (var index in EnabledVertexAttribArrays)
            {
                GL.DisableVertexAttribArray(index);
            }
            EnabledVertexAttribArrays.Clear();
        }

        /// <summary>
        /// The vertex attributes location within the shader.
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Default binding parameters attributed to this vertex attribute.
        /// </summary>
        protected readonly VertexAttribAttribute Parameters;

        internal VertexAttrib(int program, string name, VertexAttribAttribute parameters)
            : base(program, name)
        {
            Parameters = parameters;
            Index = GL.GetAttribLocation(program, name);
            if (Index == -1) Logger.WarnFormat("Vertex attribute not found or not active: {0}", name);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute. Uses the buffers element size as the stride parameter with an offset of zero.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer)
            where T : struct
        {
            Bind(buffer, Parameters.Components, Parameters.Type, buffer.ElementSize, 0, Parameters.Normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute. Uses the buffers element size as the stride parameter and the given offset.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int offset)
            where T : struct
        {
            Bind(buffer, Parameters.Components, Parameters.Type, buffer.ElementSize, offset, Parameters.Normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute. Uses the given stride and offset parameters.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
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

        /// <summary>
        /// Binds the given buffer to this vertex attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int components, VertexAttribPointerType type, int stride, int offset, bool normalized)
            where T : struct
        {
            if (Index == -1) return;
            // bind given buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            // make sure the vertex attribute is enabled
            GL.EnableVertexAttribArray(Index);
            // remember which vertex attributes are enabled
            EnabledVertexAttribArrays.Add(Index);
            // set the vertex attribute pointer to the current buffer
            GL.VertexAttribPointer(Index, components, type, normalized, stride, offset);
        }
    }
}