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
using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a cubemap texture array.<br/>
    /// Images in this texture are all cube maps. It contains multiple sets of cube maps, all within one texture.
    /// The array length * 6 (number of cube faces) is part of the texture size.
    /// </summary>
    public sealed class TextureCubemapArray
        : LayeredTexture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.TextureCubeMapArray; } }

        /// <summary>
        /// The size of the texture.<br/>
        /// This represents both width and height of the texture, because cube maps have to be square.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The number of layers.
        /// </summary>
        public int Layers { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="size">The width and height of the cube map faces.</param>
        /// <param name="layers">The number of layers to allocate.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public TextureCubemapArray(SizedInternalFormat internalFormat, int size, int layers, int levels = 0)
            : base(internalFormat, GetLevels(levels, size))
        {
            Size = size;
            Layers = layers;
            GL.BindTexture(TextureTarget, Handle);
            // note: the depth parameter is the number of layer-faces hence the multiplication by six,
            // see https://www.opengl.org/wiki/Texture_Storage#Immutable_storage
            GL.TexStorage3D((TextureTarget3d)TextureTarget, Levels, InternalFormat, Size, Size, 6 * Layers);
            CheckError();
        }
    }
}