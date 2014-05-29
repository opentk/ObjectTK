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

        public void BindImageLayer(int imageUnit, Texture texture, int layer, TextureAccess textureAccess, SizedInternalFormat format)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, texture.TextureHandle, 0, false, layer, textureAccess, format);
        }

        public void BindImageLayered(int imageUnit, Texture texture, TextureAccess textureAccess, SizedInternalFormat format = SizedInternalFormat.R32f)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, texture.TextureHandle, 0, true, 0, textureAccess, format);
        }
    }
}