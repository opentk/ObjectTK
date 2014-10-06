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
using DerpGL.Textures;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a typed texture uniform. Allows only textures of the given type to be bound.
    /// </summary>
    public class TextureUniform<T>
        : Uniform<int>
        where T : Texture
    {
        internal TextureUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        /// <summary>
        /// Sets this uniform to sample from the given texture unit.<br/>
        /// Calls to <see cref="Set(TextureUnit)"/> are equivalent to <see cref="Uniform{T}.Set"/>
        /// with the corresponding integer, it just adds readability.
        /// </summary>
        /// <param name="unit">The texture unit to sample from.</param>
        public void Set(TextureUnit unit)
        {
            Set((int)unit - (int)TextureUnit.Texture0);
        }

        /// <summary>
        /// Binds a texture to the given texture unit and sets the corresponding uniform to the respective number to access it.
        /// </summary>
        /// <param name="unit">The texture unit to bind to.</param>
        /// <param name="texture">The texture to bind.</param>
        public void BindTexture(TextureUnit unit, T texture)
        {
            Set(unit);
            texture.Bind(unit);
        }
    }

    /// <summary>
    /// Represents a texture uniform. Allows any texture type to be bound.
    /// </summary>
    public sealed class TextureUniform
        : TextureUniform<Texture>
    {
        internal TextureUniform(int program, string name)
            : base(program, name)
        {
        }
    }
}