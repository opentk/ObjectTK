//
// TextureRectangle.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Textures
{
    /// <summary>
    /// Represents a rectangle texture.<br/>
    /// The image in this texture (only one image. No mipmapping) is 2-dimensional.
    /// Texture coordinates used for these textures are not normalized.
    /// </summary>
    public sealed class TextureRectangle
        : Texture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.TextureRectangle; } }
        public override bool SupportsMipmaps { get { return false; } }

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        public TextureRectangle(SizedInternalFormat internalFormat, int width, int height)
            : base(internalFormat, 1)
        {
            Width = width;
            Height = height;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage2D((TextureTarget2d)TextureTarget, 1, InternalFormat, Width, Height);
            CheckError();
        }
    }
}