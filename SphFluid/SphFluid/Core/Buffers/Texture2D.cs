using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Texture2D
        : Texture
    {
        public void Initialize(PixelInternalFormat format, int width, int height, PixelFormat pixelFormat, PixelType pixelType)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, format, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
            Utility.Assert("Unable to create texture");
        }
    }
}