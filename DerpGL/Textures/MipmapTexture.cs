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
        /// The default GenerateMipmapTarget.
        /// </summary>
        public GenerateMipmapTarget GenerateMipmapTarget { get; private set; }

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
            : this(textureTarget, internalFormat, 0, 1)
        {
        }

        /// <summary>
        /// Initializes a new texture object which is capable of mipmapping.
        /// </summary>
        /// <param name="textureTarget">The default texture target to use.</param>
        /// <param name="internalFormat">The internal format of the texture.</param>
        /// <param name="generateMipmapTarget">The target to generate mipmaps with.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        internal MipmapTexture(TextureTarget textureTarget,  SizedInternalFormat internalFormat, GenerateMipmapTarget generateMipmapTarget, int levels)
            : base(textureTarget, internalFormat)
        {
            GenerateMipmapTarget = generateMipmapTarget;
            Levels = levels;
        }

        /// <summary>
        /// Automatically generates all mipmaps.
        /// </summary>
        public void GenerateMipMaps()
        {
            if (Levels > 1) GL.GenerateMipmap(GenerateMipmapTarget);
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