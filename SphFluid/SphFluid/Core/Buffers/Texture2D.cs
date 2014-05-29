using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Texture2D
        : Texture
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public void Initialize(SizedInternalFormat internalFormat, int width, int height)
        {
            SizedInternalFormat = internalFormat;
            Width = width;
            Height = height;
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, internalFormat, width, height);
            CheckError();
        }
    }
}