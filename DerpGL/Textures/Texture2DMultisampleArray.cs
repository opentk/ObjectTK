using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D multisample array texture.<br/>
    /// Combines 2D array and 2D multisample types. No mipmapping.
    /// </summary>
    public class Texture2DMultisampleArray
        : LayeredTexture
    {
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
            : base(TextureTarget.Texture2DMultisample, internalFormat)
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