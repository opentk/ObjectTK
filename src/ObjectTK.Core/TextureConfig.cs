using System;
using System.ComponentModel;
using JetBrains.Annotations;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK {

    public sealed class TextureConfig {

        public TextureMagFilter MagFilter { get; set; } = TextureMagFilter.Linear;
        public TextureMinFilter MinFilter { get; set; } = TextureMinFilter.LinearMipmapNearest;

        /// The format the OpenGL stores this texture data in, internally.
        /// By default, this is an RGBA 8BPP (8 bits per pixel) format.
        public PixelInternalFormat InternalFormat { get; set; } = PixelInternalFormat.Rgba;
        
        /// The format of the data used to create this texture.
        public PixelFormat PixelFormat { get; set; } = PixelFormat.Bgra;
        /// The data type of each pixel's channel used to create this texture.
        public PixelType PixelType { get; set; } = PixelType.UnsignedByte;

        /// If mipmaps should be generated for this texture (if applicable).
        public bool GenerateMipmaps { get; set; } = true;

        /// The default texture configuration. Should be good for most cases.
        [NotNull]
        public static TextureConfig Default => new TextureConfig();

        /// Creates a copy of this <see cref="TextureConfig"/>
        [Pure]
        [NotNull]
        public TextureConfig Copy() {
            return new TextureConfig {
                GenerateMipmaps = GenerateMipmaps,
                InternalFormat = InternalFormat,
                MagFilter = MagFilter,
                MinFilter = MinFilter,
                PixelFormat = PixelFormat,
                PixelType = PixelType
            };
        }
        
        // Hide the default members of this object for a cleaner API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            // ReSharper disable once BaseObjectEqualsIsObjectEquals
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        // ReSharper disable once AnnotateCanBeNullTypeMember
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [NotNull]
        public new Type GetType() {
            return base.GetType();
        }
    }
    
}
