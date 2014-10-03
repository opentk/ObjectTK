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

        /// <summary>
        /// Binds the texture to the current texture unit at its default texture target.
        /// </summary>
        public void Bind()
        {
            GL.BindTexture(TextureTarget, Handle);
        }

        /// <summary>
        /// Binds the texture to the given texture unit at its default texture target.
        /// </summary>
        /// <param name="unit">The texture unit to bind to.</param>
        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            Bind();
        }

        /// <summary>
        /// Sets the given wrap mode on all dimensions R, S and T.
        /// </summary>
        /// <param name="wrapMode">The wrap mode to apply.</param>
        public void SetWrapMode(TextureWrapMode wrapMode)
        {
            var mode = (int)wrapMode;
            SetParameter(TextureParameterName.TextureWrapR, mode);
            SetParameter(TextureParameterName.TextureWrapS, mode);
            SetParameter(TextureParameterName.TextureWrapT, mode);
        }

        /// <summary>
        /// Sets texture parameters.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void SetParameter(TextureParameterName parameterName, int value)
        {
            GL.TexParameter(TextureTarget, parameterName, value);
        }

        /// <summary>
        /// Calls GL.<see cref="GL.GetError()"/> to check if there are any errors.
        /// </summary>
        protected static void CheckError()
        {
#if DEBUG
            Utility.Assert("Unable to create texture");
#endif
        }
    }
}