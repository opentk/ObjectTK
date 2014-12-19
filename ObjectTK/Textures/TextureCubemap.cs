#region License
// ObjectTK License
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
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Textures
{
    /// <summary>
    /// Represents a cubemap texture.<br/>
    /// There are exactly 6 distinct sets of 2D images, all of the same size. They act as 6 faces of a cube.
    /// </summary>
    public sealed class TextureCubemap
        : LayeredTexture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.TextureCubeMap; } }

        /// <summary>
        /// The size of the texture.<br/>
        /// This represents both width and height of the texture, because cube maps have to be square.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="size">The width and height of the cube map faces.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public TextureCubemap(SizedInternalFormat internalFormat, int size, int levels = 0)
            : base(internalFormat, GetLevels(levels, size))
        {
            Size = size;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage2D((TextureTarget2d)TextureTarget, Levels, InternalFormat, Size, Size);
            CheckError();
        }
    }
}
