#region License
// DerpGL License
// Copyright (C) 2013-2014 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using System.Linq;
using System.Reflection;
using DerpGL.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a vertex attribute.
    /// </summary>
    public sealed class VertexAttrib
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VertexAttrib));

        /// <summary>
        /// The vertex attributes location within the shader.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Default binding parameters attributed to this vertex attribute.
        /// </summary>
        private VertexAttribAttribute _parameters;

        static VertexAttrib()
        {
            AddTypedCallback(new ShaderVariableCallback<VertexAttrib>((_, i) => _.GetAttributes(i)));
        }

        internal VertexAttrib()
        {
            PostLink += OnPostLink;
        }

        private void GetAttributes(PropertyInfo property)
        {
            _parameters = property.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault() ?? new VertexAttribAttribute();
        }
        //    new Mapping<VertexAttrib>((p,i) => new VertexAttrib(p, i.Name, i.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault() ?? new VertexAttribAttribute())),

        internal void OnPostLink()
        {
            Index = GL.GetAttribLocation(Program, Name);
            if (Index == -1) Logger.WarnFormat("Vertex attribute not found or not active: {0}", Name);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute. Uses the buffers element size as the stride parameter with an offset of zero.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer)
            where T : struct
        {
            Bind(buffer, _parameters.Components, _parameters.Type, buffer.ElementSize, 0, _parameters.Normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute. Uses the buffers element size as the stride parameter and the given offset.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int offset)
            where T : struct
        {
            Bind(buffer, _parameters.Components, _parameters.Type, buffer.ElementSize, offset, _parameters.Normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute. Uses the given stride and offset parameters.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int stride, int offset)
            where T : struct
        {
            Bind(buffer, _parameters.Components, _parameters.Type, stride, offset, _parameters.Normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int stride, int offset, bool normalized)
            where T : struct
        {
            Bind(buffer, _parameters.Components, _parameters.Type, stride, offset, normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int components, int stride, int offset)
            where T : struct
        {
            Bind(buffer, components, _parameters.Type, stride, offset, _parameters.Normalized);
        }

        /// <summary>
        /// Binds the given buffer to this vertex attribute.
        /// </summary>
        public void Bind<T>(Buffer<T> buffer, int components, int stride, int offset, bool normalized)
            where T : struct
        {
            Bind(buffer, components, _parameters.Type, stride, offset, normalized);
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
            // set the vertex attribute pointer to the current buffer
            GL.VertexAttribPointer(Index, components, type, normalized, stride, offset);
        }
    }
}