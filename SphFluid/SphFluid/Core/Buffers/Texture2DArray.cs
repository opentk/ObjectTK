using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Texture2DArray
        : Texture
    {
        public void Initialize(PixelInternalFormat internalFormat, int width, int height, int layers, PixelFormat pixelFormat, PixelType pixelType)
        {
            PixelInternalFormat = internalFormat;
            GL.BindTexture(TextureTarget.Texture2DArray, TextureHandle);
            GL.TexImage3D(TextureTarget.Texture2DArray, 0, internalFormat, width, height, layers, 0, pixelFormat, pixelType, IntPtr.Zero);
            CheckError();
        }
    }
}