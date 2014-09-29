using DerpGL.Textures;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a texture uniform.
    /// </summary>
    public class TextureUniform
        : Uniform<int>
    {
        internal TextureUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        /// <summary>
        /// Binds a <see cref="Texture"/> to the given <see cref="TextureUnit"/> and sets the corresponding uniform to the respective number to access it.
        /// </summary>
        /// <param name="target">The target to which the texture is bound.</param>
        /// <param name="unit">The texture unit to bind to.</param>
        /// <param name="texture">The texture to bind.</param>
        public void BindTexture(TextureTarget target, TextureUnit unit, Texture texture)
        {
            const int zero = (int)TextureUnit.Texture0;
            if (!Set((int)unit - zero)) return;
            GL.ActiveTexture(unit);
            GL.BindTexture(target, texture.Handle);
        }

        /// <summary>
        /// Binds a <see cref="Texture"/> to the given <see cref="TextureUnit"/> and sets the corresponding uniform to the respective number to access it.<br/>
        /// The <see cref="TextureTarget"/> is taken from the texture.
        /// </summary>
        /// <param name="unit">The texture unit to bind to.</param>
        /// <param name="texture">The texture to bind.</param>
        public void BindTexture(TextureUnit unit, Texture texture)
        {
            BindTexture(texture.TextureTarget, unit, texture);
        }
    }
}