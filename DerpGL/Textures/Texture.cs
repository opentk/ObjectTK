using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents an OpenGL texture.
    /// </summary>
    public abstract class Texture
        : GLResource
    {
        /// <summary>
        /// The default texture target.
        /// </summary>
        public TextureTarget TextureTarget { get; private set; }

        /// <summary>
        /// The internal format of the texture.
        /// </summary>
        public SizedInternalFormat InternalFormat { get; private set; }

        /// <summary>
        /// Initializes a new texture object which is capable of mipmapping.
        /// </summary>
        /// <param name="textureTarget">The default texture target to use.</param>
        /// <param name="internalFormat">The internal format of the texture.</param>
        internal Texture(TextureTarget textureTarget, SizedInternalFormat internalFormat)
            : base(GL.GenTexture())
        {
            TextureTarget = textureTarget;
            InternalFormat = internalFormat;
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteTexture(Handle);
        }

        protected static void CheckError()
        {
#if DEBUG
            Utility.Assert("Unable to create texture");
#endif
        }
    }
}