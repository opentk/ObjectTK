#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
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