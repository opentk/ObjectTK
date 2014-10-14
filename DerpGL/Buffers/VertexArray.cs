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
using System;
using DerpGL.Exceptions;
using DerpGL.Shaders.Variables;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    /// <summary>
    /// Represents a vertex array object.
    /// </summary>
    public class VertexArray
        : GLResource
    {
        /// <summary>
        /// Initializes a new vertex array object.
        /// </summary>
        public VertexArray()
            : base(GL.GenVertexArray())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteVertexArray(Handle);
        }

        /// <summary>
        /// Bind the vertex array.
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        /// <summary>
        /// Render primitives from array data.
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render.</param>
        /// <param name="count">Specifies the number of indices to be rendered.</param>
        public void DrawArrays(PrimitiveType mode, int count)
        {
            DrawArrays(mode, 0, count);
        }

        /// <summary>
        /// Render primitives from array data.
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render.</param>
        /// <param name="first">Specifies the starting index in the enabled arrays.</param>
        /// <param name="count">Specifies the number of indices to be rendered.</param>
        public void DrawArrays(PrimitiveType mode, int first, int count)
        {
            AssertActive();
            GL.DrawArrays(mode, first, count);
        }

        /// <summary>
        /// Render primitives from array data using the element buffer.
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render.</param>
        /// <param name="count">Specifies the number of elements to be rendered.</param>
        /// <param name="type">Specifies the type of the values in indices.</param>
        public void DrawElements(PrimitiveType mode, int count, DrawElementsType type = DrawElementsType.UnsignedInt)
        {
            AssertActive();
            GL.DrawElements(mode, count, type, IntPtr.Zero);
        }

        /// <summary>
        /// Binds the given buffer to the element array buffer target.
        /// </summary>
        public void BindElements<T>(Buffer<T> buffer)
            where T : struct
        {
            AssertActive();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.Handle);
        }

        /// <summary>
        /// Unbinds any buffer bound to the element array buffer target.
        /// </summary>
        public void UnbindElements()
        {
            AssertActive();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute. Uses the buffers element size as the stride parameter with an offset of zero.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer)
            where T : struct
        {
            BindAttribute(attribute, buffer, attribute.Components, attribute.Type, buffer.ElementSize, 0, attribute.Normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute. Uses the buffers element size as the stride parameter and the given offset.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer, int offset)
            where T : struct
        {
            BindAttribute(attribute, buffer, attribute.Components, attribute.Type, buffer.ElementSize, offset, attribute.Normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute. Uses the given stride and offset parameters.
        /// The other parameters, namely components, type and normalized are chosen according to the corresponding <see cref="VertexAttribAttribute"/> attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer, int stride, int offset)
            where T : struct
        {
            BindAttribute(attribute, buffer, attribute.Components, attribute.Type, stride, offset, attribute.Normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer, int stride, int offset, bool normalized)
            where T : struct
        {
            BindAttribute(attribute, buffer, attribute.Components, attribute.Type, stride, offset, normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer, int components, int stride, int offset)
            where T : struct
        {
            BindAttribute(attribute, buffer, components, attribute.Type, stride, offset, attribute.Normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer, int components, int stride, int offset, bool normalized)
            where T : struct
        {
            BindAttribute(attribute, buffer, components, attribute.Type, stride, offset, normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute.
        /// </summary>
        public void BindAttribute<T>(VertexAttrib attribute, Buffer<T> buffer, int components, VertexAttribPointerType type, int stride, int offset, bool normalized)
            where T : struct
        {
            if (!attribute.Active) return;
            BindAttribute(attribute.Index, buffer, components, type, stride, offset, normalized);
        }

        /// <summary>
        /// Binds the buffer to the given vertex attribute.
        /// </summary>
        public void BindAttribute<T>(int index, Buffer<T> buffer, int components, VertexAttribPointerType type, int stride, int offset, bool normalized)
            where T : struct
        {
            AssertActive();
            // bind given buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            // make sure the vertex attribute is enabled
            GL.EnableVertexAttribArray(index);
            // set the vertex attribute pointer to the current buffer
            GL.VertexAttribPointer(index, components, type, normalized, stride, offset);
        }

        /// <summary>
        /// Disable the given vertex attribute.
        /// </summary>
        public void UnbindAttribute(VertexAttrib attribute)
        {
            UnbindAttribute(attribute.Index);
        }

        /// <summary>
        /// Disable the given vertex attribute.
        /// </summary>
        public void UnbindAttribute(int index)
        {
            AssertActive();
            GL.DisableVertexAttribArray(index);
        }

        /// <summary>
        /// Throws an <see cref="ObjectNotBoundException"/> if this vertex array is not the currently active one.
        /// </summary>
        public void AssertActive()
        {
#if DEBUG
            int activeHandle;
            GL.GetInteger(GetPName.VertexArrayBinding, out activeHandle);
            if (activeHandle != Handle) throw new ObjectNotBoundException("Vertex array object is not bound.");
#endif
        }
    }
}