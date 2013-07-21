using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Texture
    {
        public int TextureHandle { get; private set; }

        public Texture()
        {
            // create texture
            TextureHandle = GL.GenTexture();
        }
    }
}