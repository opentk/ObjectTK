using OpenTK.Graphics.OpenGL;

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
        /// <param name="internalFormat"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public Texture3D(SizedInternalFormat internalFormat, int width, int height, int depth)
            : base(TextureTarget.Texture3D, internalFormat)
        {
            Initialize(width, height, depth);
        }

        /// <summary>
        /// Initializes a new 3D texture object.
        /// </summary>
        public Texture3D(SizedInternalFormat internalFormat, int width, int height, int depth, int levels)
            : base(TextureTarget.Texture3D, internalFormat, GenerateMipmapTarget.Texture3D, levels)
        {
            Initialize(width, height, depth);
        }

        private void Initialize(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, Levels, InternalFormat, Width, Height, Depth);
            CheckError();
            SetDefaultTexParameters();
        }
    }
}