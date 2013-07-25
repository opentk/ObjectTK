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

        protected void SetTexParameters()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }
    }
}