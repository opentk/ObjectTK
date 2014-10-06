#region License
// DerpGL License
// Copyright (C) 2013-2014 J.C.Bernack
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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D texture array.<br/>
    /// Images in this texture all are 2-dimensional. However, it contains multiple sets of 2-dimensional images,
    /// all within one texture. The array length is part of the texture's size.
    /// </summary>
    public sealed class Texture2DArray
        : LayeredTexture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture2DArray; } }

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The number of layers.
        /// </summary>
        public int Layers { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.<br/>
        /// A value of zero for the number of mipmap levels will default to the maximum number of levels possible for the given bitmaps width and height.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="layers">The number of layers to allocate.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture2DArray(SizedInternalFormat internalFormat, int width, int height, int layers, int levels = 0)
            : base(internalFormat, GetLevels(levels, width, height))
        {
            Width = width;
            Height = height;
            Layers = layers;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage3D((TextureTarget3d)TextureTarget, Levels, InternalFormat, Width, Height, Layers);
            CheckError();
        }

        /// <summary>
        /// Creates a new Texture2DArray instance compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">Specifies the bitmap to which the new texture will be compatible.</param>
        /// <param name="layers">Specifies the number of array layers the texture will contain.</param>
        /// <param name="levels">Specifies the number of mipmap levels.</param>
        /// <returns>A new texture instance.</returns>
        public static Texture2DArray CreateCompatible(Bitmap bitmap, int layers, int levels = 0)
        {
            return new Texture2DArray(FormatMapping.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height, layers, levels);
        }
    }
}