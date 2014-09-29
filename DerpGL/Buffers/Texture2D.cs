using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class Texture2D
        : Texture
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// DEBUG: Retrieves the texture data interpreted as R32ui
        /// </summary>
        public uint[,] ContentR32Ui
        {
            get
            {
                var data = new uint[Width,Height];
                GL.BindTexture(TextureTarget.Texture2D, Handle);
                GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.RedInteger, PixelType.UnsignedInt, data);
                return data;
            }
        }

        public Texture2D(SizedInternalFormat internalFormat, int width, int height)
            : base(internalFormat)
        {
            Width = width;
            Height = height;
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, internalFormat, width, height);
            CheckError();
        }
    }
}