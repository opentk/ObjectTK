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
        /// The default texture target for this texture.
        /// </summary>
        public TextureTarget TextureTarget { get; protected set; }

        /// <summary>
        /// The internal format of the texture.
        /// </summary>
        public SizedInternalFormat InternalFormat { get; protected set; }

        protected Texture(TextureTarget textureTarget, SizedInternalFormat internalFormat)
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

        protected void SetDefaultTexParameters()
        {
            GL.TexParameter(TextureTarget, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            CheckError();
        }
    }
}