using DerpGL.Textures;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a texture uniform.
    /// </summary>
    public sealed class TextureUniform
        : Uniform<int>
    {
        internal TextureUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        /// <summary>
        /// Sets this uniform to sample from the given texture unit.
        /// </summary>
        /// <param name="unit">The texture unit to sample from.</param>
        public void Set(TextureUnit unit)
        {
            Set((int)unit - (int)TextureUnit.Texture0);
        }

        /// <summary>
        /// Binds a <see cref="Texture"/> to the given <see cref="TextureUnit"/> and sets the corresponding uniform to the respective number to access it.<br/>
        /// The <see cref="TextureTarget"/> is taken from the texture.
        /// </summary>
        /// <param name="unit">The texture unit to bind to.</param>
        /// <param name="texture">The texture to bind.</param>
        public void BindTexture(TextureUnit unit, Texture texture)
        {
            Set(unit);
            texture.Bind(unit);
        }
    }
}