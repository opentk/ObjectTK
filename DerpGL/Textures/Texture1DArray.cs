using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 1D texture array.
    /// </summary>
    public sealed class Texture1DArray
        : LayeredTexture
    {
        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The number of layers.<br/>
        /// note: OpenGL seems to call the second coordinate on a 1D texture array the "height",
        /// which would make the whole thing almost exactly equal to a 2D texture with the exception that
        /// a 1D texture array can be bound to a framebuffer via glFramebufferTextureLayer().
        /// </summary>
        public int Layers { get; private set; }

        /// <summary>
        /// Creates a 1D texture array with given internal format, width and number of layers.
        /// </summary>
        public Texture1DArray(SizedInternalFormat internalFormat, int width, int layers)
            : base(TextureTarget.Texture1DArray, internalFormat)
        {
            Initialize(width, layers);
        }

        /// <summary>
        /// Creates a 1D texture array with given internal format, width, number of layers and number of mipmap levels.
        /// </summary>
        public Texture1DArray(SizedInternalFormat internalFormat, int width, int layers, int levels)
            : base(TextureTarget.Texture1DArray, internalFormat, levels)
        {
            Initialize(width, layers);
        }

        private void Initialize(int width, int layers)
        {
            Width = width;
            Layers = layers;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage2D((TextureTarget2d)TextureTarget, Levels, InternalFormat, Width, Layers);
            CheckError();
        }
    }
}