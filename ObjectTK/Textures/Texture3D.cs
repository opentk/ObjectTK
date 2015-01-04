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
    /// Represents a 3D texture.<br/>
    /// Images in this texture all are 3-dimensional. They have width, height, and depth.
    /// </summary>
    public sealed class Texture3D
        : LayeredTexture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture3D; } }

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The depth of the texture.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="depth">The depth of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture3D(SizedInternalFormat internalFormat, int width, int height, int depth, int levels = 0)
            : base(internalFormat, GetLevels(levels, width, height, depth))
        {
            Width = width;
            Height = height;
            Depth = depth;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage3D((TextureTarget3d)TextureTarget, Levels, InternalFormat, Width, Height, Depth);
            CheckError();
        }
    }
}