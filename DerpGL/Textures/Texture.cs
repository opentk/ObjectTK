using System;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents an OpenGL texture.
    /// </summary>
    /// <remarks>
    /// <code>
    /// Type              Supports: Mipmaps Layered
    /// -------------------------------------------
    /// Texture1D                   yes
    /// Texture2D                   yes
    /// Texture3D                   yes     yes
    /// Texture1DArray              yes     yes
    /// Texture2DArray              yes     yes
    /// TextureCubemap              yes     yes
    /// TextureCubemapArray         yes     yes
    /// Texture2DMultisample
    /// Texture2DMultisampleArray           yes
    /// TextureRectangle
    /// TextureBuffer
    /// </code>
    /// </remarks>
    public abstract class Texture
        : GLResource
    {
        /// <summary>
        /// Specifies the texture target.
        /// </summary>
        public abstract TextureTarget TextureTarget { get; }

        /// <summary>
        /// Specifies whether this texture supports multiple layers.<br/>
        /// True for all texture types derived from LayeredTexture, that is all array, cube map and 3D textures.
        /// </summary>
        public virtual bool SupportsLayers { get { return false; } }

        /// <summary>
        /// Specifies whether this texture supports mipmap levels.<br/>
        /// False for buffer, rectangle and multisample textures, otherwise true.
        /// </summary>
        public virtual bool SupportsMipmaps { get { return true; } }

        /// <summary>
        /// The number of mipmap levels.
        /// </summary>
        public int Levels { get; private set; }

        /// <summary>
        /// The internal format of the texture.
        /// </summary>
        public SizedInternalFormat InternalFormat { get; private set; }

        /// <summary>
        /// Initializes a new texture object. Creates a new texture handle.
        /// </summary>
        /// <param name="internalFormat">The internal format of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        internal Texture(SizedInternalFormat internalFormat, int levels)
            : this(GL.GenTexture(), internalFormat, levels)
        {
        }

        /// <summary>
        /// Initializes a new texture object. Uses the texture handle given.<br/>
        /// Internal constructor used by <see cref="TextureFactory"/> to wrap a texture instance around an already existing texture.
        /// </summary>
        /// <param name="textureHandle">The texture handle.</param>
        /// <param name="internalFormat">The internal format of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        internal Texture(int textureHandle, SizedInternalFormat internalFormat, int levels)
            : base(textureHandle)
        {
            InternalFormat = internalFormat;
            Levels = levels;
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
        /// Automatically generates all mipmaps.
        /// </summary>
        public void GenerateMipMaps()
        {
            if (!SupportsMipmaps || Levels <= 1) return;
            Bind();
            GL.GenerateMipmap((GenerateMipmapTarget)TextureTarget);
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
        /// Sets default texture parameters to ensure texture completeness.
        /// </summary>
        public virtual void SetDefaultTexParameters()
        {
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, (int)(Levels > 1 ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Linear));
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            CheckError();
        }

        /// <summary>
        /// Checks if the given mipmap level is supported by this texture.<br/>
        /// A supported level is either zero for all textures which do not support mipmapping,
        /// or smaller than the number of existing levels.
        /// </summary>
        /// <param name="level">The mipmap level of the texture.</param>
        /// <returns>True if the level is supported, otherwise false.</returns>
        public bool SupportsLevel(int level)
        {
            return (SupportsMipmaps || level == 0) && (level < Levels || !SupportsMipmaps);
        }

        /// <summary>
        /// Throws an exception if the given mipmap level is not supported by this texture.
        /// </summary>
        /// <param name="level">The mipmap level of the texture.</param>
        internal void AssertLevel(int level)
        {
#if DEBUG
            if (!SupportsLevel(level)) throw new ArgumentException("Texture does not support this mipmap level or mipmapping at all.");
#endif
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