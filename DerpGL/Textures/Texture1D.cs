using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 1D texture.<br/>
    /// Images in this texture all are 1-dimensional. They have width, but no height or depth.
    /// </summary>
    public sealed class Texture1D
        : Texture
    {
        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        public Texture1D(SizedInternalFormat internalFormat, int width)
            : base(TextureTarget.Texture1DArray, internalFormat, 1)
        {
            Initialize(width);
        }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
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