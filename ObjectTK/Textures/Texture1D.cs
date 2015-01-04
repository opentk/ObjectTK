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
    /// Represents a 1D texture.<br/>
    /// Images in this texture all are 1-dimensional. They have width, but no height or depth.
    /// </summary>
    public sealed class Texture1D
        : Texture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture1D; } }

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture1D(SizedInternalFormat internalFormat, int width, int levels = 0)
            : base(internalFormat, GetLevels(levels, width))
        {
            Width = width;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage1D((TextureTarget1d)TextureTarget, Levels, InternalFormat, Width);
            CheckError();
        }
    }
}