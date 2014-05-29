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

        public void BindImage(int imageUnit, Texture texture, bool layered, int layer, TextureAccess access, SizedInternalFormat format)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, texture.TextureHandle, 0, layered, layer, access, format);
        }

        public void BindImage(int imageUnit, Texture texture, bool layered, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture, layered, layer, access, texture.SizedInternalFormat);
        }

        public void BindImage(int imageUnit, Texture2D texture, TextureAccess access)
        {
            BindImage(imageUnit, texture, false, 0, access, texture.SizedInternalFormat);
        }

        public void BindImage(int imageUnit, Texture2DArray texture, TextureAccess access)
        {
            BindImage(imageUnit, texture, true, 0, access, texture.SizedInternalFormat);
        }
    }
}