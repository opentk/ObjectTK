#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using System;
using ObjectTK.Buffers;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Textures
{
    /// <summary>
    /// Represents a buffer texture.<br/>
    /// The image in this texture (only one image. No mipmapping) is 1-dimensional.
    /// The storage for this data comes from a Buffer Object.
    /// </summary>
    public sealed class TextureBuffer
        : Texture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.TextureBuffer; } }
        public override bool SupportsMipmaps { get { return false; } }

        /// <summary>
        /// Creates a buffer texture and uses the given internal format to access a bound buffer, if not specified otherwise.
        /// </summary>
        /// <param name="internalFormat"></param>
        public TextureBuffer(SizedInternalFormat internalFormat)
            : base(internalFormat, 1)
        {
        }

        /// <summary>
        /// Binds the given buffer to this texture.<br/>
        /// Applies the internal format specified in the constructor.
        /// </summary>
        /// <param name="buffer">The buffer to bind.</param>
        public void BindBufferToTexture<T>(Buffer<T> buffer)
            where T : struct
        {
            BindBufferToTexture(buffer, InternalFormat);
        }

        /// <summary>
        /// Binds the given buffer to this texture using the given internal format.
        /// </summary>
        /// <param name="buffer">The buffer to bind.</param>
        /// <param name="internalFormat">The internal format used when accessing the buffer.</param>
        /// <typeparam name="T">The type of elements in the buffer object.</typeparam>
        public void BindBufferToTexture<T>(Buffer<T> buffer, SizedInternalFormat internalFormat)
            where T : struct
        {
            if (!buffer.Initialized) throw new ArgumentException("Can not bind uninitialized buffer to buffer texture.", "buffer");
            GL.BindTexture(TextureTarget.TextureBuffer, Handle);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, internalFormat, buffer.Handle);
        }
    }
}