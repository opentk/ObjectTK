//
// Texture.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Textures
{
    /// <summary>
    /// Represents a texture object.
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
        : GLObject
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
        /// Calculates the maximum number of mipmap levels allowed for the given size in each dimension.<br/>
        /// If <paramref name="levels"/> is greater than zero and less or equal to the calculated maximum it is returned without change.<br/>
        /// If <paramref name="levels"/> is zero the calculated maximum is returned instead.
        /// </summary>
        /// <remarks>
        /// At the maximum mipmap level the image would consist of exactly one texel, i.e. 1x1 in 2D or 1x1x1 in 3D.
        /// </remarks>
        /// <param name="levels">Specifies the number of desired mipmap levels.</param>
        /// <param name="dimensions">Specifies the size of the textures base image in each dimension.</param>
        /// <returns>A valid number of mipmap levels.</returns>
        protected static int GetLevels(int levels, params int[] dimensions)
        {
            var maxLevels = TextureFactory.CalculateMaxMipmapLevels(dimensions);
            if (levels > maxLevels || levels < 0) throw new ArgumentOutOfRangeException("levels", levels,
                string.Format("The valid range of mipmapping levels for a maximum texture dimension of {0} is [0,{1}]", dimensions.Max(), maxLevels));
            return levels == 0 ? maxLevels : levels;
        }

        /// <summary>
        /// Automatically generates all mipmaps.
        /// </summary>
        public void GenerateMipMaps()
        {
            if (!SupportsMipmaps) throw new InvalidOperationException("Texture does not support mipmaps.");
            Bind();
            GL.GenerateMipmap((GenerateMipmapTarget)TextureTarget);
            Utility.Assert("Could not generate mipmaps.");
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
        /// Sets the given texture minification and magnification filters.
        /// </summary>
        /// <param name="minFilter"></param>
        /// <param name="magFilter"></param>
        public void SetFilter(TextureMinFilter minFilter, TextureMagFilter magFilter)
        {
            SetParameter(TextureParameterName.TextureMinFilter, (int)minFilter);
            SetParameter(TextureParameterName.TextureMagFilter, (int)magFilter);
        }

        /// <summary>
        /// Sets default texture parameters to ensure texture completeness.<br/>
        /// Enables mipmapping if the texture supports it, otherwise filtering is set to linear interpolation.
        /// </summary>
        public virtual void SetDefaultTexParameters()
        {
            SetParameter(TextureParameterName.TextureMinFilter, (int)(Levels > 1 ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Linear));
            SetParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            Utility.Assert("Could not set tex parameters.");
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
        /// Throws an exception if the given mipmap level is not supported by this texture.<br/>
        /// The mipmap level must be zero for all texture types which do not support mipmaps.
        /// </summary>
        /// <param name="level">Specifies a mipmap level of the texture.</param>
        internal void AssertLevel(int level)
        {
            if (!SupportsLevel(level)) throw new ArgumentException(string.Format("Texture does not contain the mipmap level {0} or does not support mipmapping at all.", level));
        }

        /// <summary>
        /// Calls GL.<see cref="GL.GetError()"/> to check if there are any errors.
        /// </summary>
        protected static void CheckError()
        {
            Utility.Assert("Texture error");
        }
    }
}