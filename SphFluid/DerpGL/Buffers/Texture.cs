using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public abstract class Texture
        : GLResource
    {
        public SizedInternalFormat InternalFormat { get; protected set; }

        protected Texture(SizedInternalFormat internalFormat)
            : base(GL.GenTexture())
        {
            InternalFormat = internalFormat;
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteTexture(Handle);
        }

        protected void SetTexParameters()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
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