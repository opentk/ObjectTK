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