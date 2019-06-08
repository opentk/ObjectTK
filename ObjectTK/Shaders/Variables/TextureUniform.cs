//
// TextureUniform.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using ObjectTK.Textures;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Represents a typed texture uniform. Allows only textures of the given type to be bound.
    /// </summary>
    public class TextureUniform<T>
        : Uniform<int>
        where T : Texture
    {
        internal TextureUniform()
            : base(GL.Uniform1)
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
    public sealed class TextureUniform : TextureUniform<Texture> { }
}