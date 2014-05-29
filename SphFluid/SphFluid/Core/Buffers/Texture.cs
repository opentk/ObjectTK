using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public abstract class Texture
        : ContextResource
    {
        public int TextureHandle { get; private set; }
        public SizedInternalFormat InternalFormat { get; private set; }

        protected Texture(SizedInternalFormat internalFormat)
        {
            InternalFormat = internalFormat;
            TextureHandle = GL.GenTexture();
        }

        protected override void OnRelease()
        {
            GL.DeleteTexture(TextureHandle);
        }

        protected void SetTexParameters()
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        protected static void CheckError()
        {
#if DEBUG
            Utility.Assert("Unable to create texture");
#endif
        }
    }
}