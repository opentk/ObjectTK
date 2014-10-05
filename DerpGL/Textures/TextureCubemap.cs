using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a cubemap texture.<br/>
    /// There are exactly 6 distinct sets of 2D images, all of the same size. They act as 6 faces of a cube.
    /// </summary>
    public class TextureCubemap
        : LayeredTexture
    {
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
        public TextureCubemap(SizedInternalFormat internalFormat, int size)
            : base(TextureTarget.TextureCubeMap, internalFormat)
        {
            Initialize(size);
        }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="size">The width and height of the cube map faces.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public TextureCubemap(SizedInternalFormat internalFormat, int size, int levels)
            : base(TextureTarget.TextureCubeMap, internalFormat, levels)
        {
            Initialize(size);
        }

        private void Initialize(int size)
        {
            Size = size;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage2D((TextureTarget2d)TextureTarget, Levels, InternalFormat, Size, Size);
            CheckError();
        }
    }
}
