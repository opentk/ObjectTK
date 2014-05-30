using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    public class ImageUniform
        : Uniform<int>
    {
        public ImageUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        public void BindImage(int imageUnit, int textureHandle, bool layered, int layer, TextureAccess access, SizedInternalFormat format)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, textureHandle, 0, layered, layer, access, format);
        }

        public void BindTexture(int imageUnit, Texture texture, bool layered, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture.TextureHandle, layered, layer, access, texture.InternalFormat);
        }

        public void BindTexture(int imageUnit, Texture2D texture, TextureAccess access)
        {
            BindImage(imageUnit, texture.TextureHandle, false, 0, access, texture.InternalFormat);
        }

        public void BindTexture(int imageUnit, Texture2DArray texture, TextureAccess access)
        {
            BindImage(imageUnit, texture.TextureHandle, true, 0, access, texture.InternalFormat);
        }

        public void BindBuffer<T>(int imageUnit, Vbo<T> buffer, TextureAccess access)
            where T : struct
        {
            BindImage(imageUnit, buffer.TextureHandle, false, 0, access, buffer.BufferTextureFormat);
        }
    }
}