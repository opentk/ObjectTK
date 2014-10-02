using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 1D texture.
    /// </summary>
    public sealed class Texture1D
        : MipmapTexture
    {
        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Creates a 1D texture array with given internal format and width.
        /// </summary>
        public Texture1D(SizedInternalFormat internalFormat, int width)
            : base(TextureTarget.Texture1DArray, internalFormat)
        {
            Initialize(width);
        }

        /// <summary>
        /// Creates a 1D texture array with given internal format, width, number of layers and number of mipmap levels.
        /// </summary>
        public Texture1D(SizedInternalFormat internalFormat, int width, int levels)
            : base(TextureTarget.Texture1DArray, internalFormat, levels)
        {
            Initialize(width);
        }

        private void Initialize(int width)
        {
            Width = width;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage1D((TextureTarget1d)TextureTarget, Levels, InternalFormat, Width);
            CheckError();
        }
    }
}