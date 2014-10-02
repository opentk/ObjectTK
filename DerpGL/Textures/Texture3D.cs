using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 3D texture.
    /// </summary>
    public sealed class Texture3D
        : LayeredTexture
    {
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
        /// Initializes a new 3D texture object.
        /// </summary>
        public Texture3D(SizedInternalFormat internalFormat, int width, int height, int depth)
            : base(TextureTarget.Texture3D, internalFormat)
        {
            Initialize(width, height, depth);
        }

        /// <summary>
        /// Initializes a new 3D texture object.
        /// </summary>
        public Texture3D(SizedInternalFormat internalFormat, int width, int height, int depth, int levels)
            : base(TextureTarget.Texture3D, internalFormat, levels)
        {
            Initialize(width, height, depth);
        }

        private void Initialize(int width, int height, int depth)
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