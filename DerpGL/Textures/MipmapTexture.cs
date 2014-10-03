using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a texture capable of mipmapping.
    /// </summary>
    public abstract class MipmapTexture
        : Texture
    {
        /// <summary>
        /// The number of mipmap levels.
        /// </summary>
        public int Levels { get; private set; }

        /// <summary>
        /// Initializes a new texture object which is capable of mipmapping, but sets the mipmap levels to one.
        /// </summary>
        /// <param name="textureTarget">The default texture target to use.</param>
        /// <param name="internalFormat">The internal format of the texture.</param>
        internal MipmapTexture(TextureTarget textureTarget, SizedInternalFormat internalFormat)
            : this(textureTarget, internalFormat, 1)
        {
        }

        /// <summary>
        /// Initializes a new texture object which is capable of mipmapping.
        /// </summary>
        /// <param name="textureTarget">The default texture target to use.</param>
        /// <param name="internalFormat">The internal format of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        internal MipmapTexture(TextureTarget textureTarget,  SizedInternalFormat internalFormat, int levels)
            : base(textureTarget, internalFormat)
        {
            Levels = levels;
        }

        /// <summary>
        /// Internal constructor used by <see cref="TextureFactory"/> to wrap a texture instance around an already existing texture.
        /// </summary>
        internal MipmapTexture(int textureHandle, TextureTarget textureTarget, SizedInternalFormat internalFormat, int levels)
            : base(textureHandle, textureTarget, internalFormat)
        {
            Levels = levels;
        }

        /// <summary>
        /// Automatically generates all mipmaps.
        /// </summary>
        public void GenerateMipMaps()
        {
            if (Levels <= 1) return;
            Bind();
            GL.GenerateMipmap((GenerateMipmapTarget)TextureTarget);
        }

        /// <summary>
        /// Sets default texture parameters to ensure texture completeness.
        /// </summary>
        public virtual void SetDefaultTexParameters()
        {
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, (int)(Levels > 1 ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Linear));
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            CheckError();
        }
    }
}