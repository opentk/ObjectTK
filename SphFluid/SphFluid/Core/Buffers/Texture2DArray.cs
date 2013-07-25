using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Texture2DArray
        : Texture
    {
        public void Initialize(PixelInternalFormat format, int width, int height, int slices, PixelFormat pixelFormat, PixelType pixelType)
        {
            GL.BindTexture(TextureTarget.Texture2DArray, TextureHandle);
            GL.TexImage3D(TextureTarget.Texture2DArray, 0, format, width, height, slices, 0, pixelFormat, pixelType, IntPtr.Zero);
            Utility.Assert("Unable to create texture");
        }
    }
}