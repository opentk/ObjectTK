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
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D multisample array texture.<br/>
    /// Combines 2D array and 2D multisample types. No mipmapping.
    /// </summary>
    public sealed class Texture2DMultisampleArray
        : LayeredTexture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture2DMultisampleArray; } }
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
        /// The number of layers.
        /// </summary>
        public int Layers { get; private set; }

        /// <summary>
        /// The number of samples per texel.
        /// </summary>
        public int Samples { get; private set; }

        /// <summary>
        /// Specifies whether the texels will use identical sample locations.
        /// </summary>
        public bool FixedSampleLocations { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="layers">The number of layers to allocate.</param>
        /// <param name="samples">The number of samples per texel.</param>
        /// <param name="fixedSampleLocations">Specifies whether the texels will use identical sample locations.</param>
        public Texture2DMultisampleArray(SizedInternalFormat internalFormat, int width, int height, int layers, int samples, bool fixedSampleLocations)
            : base(internalFormat, 1)
        {
            Width = width;
            Height = height;
            Layers = layers;
            Samples = samples;
            FixedSampleLocations = fixedSampleLocations;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage3DMultisample((TextureTargetMultisample3d)TextureTarget, Samples, InternalFormat, Width, Height, Layers, FixedSampleLocations);
            CheckError();
        }
    }
}