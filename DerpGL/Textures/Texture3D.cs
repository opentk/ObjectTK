using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    public class Texture3D
        : LayeredTexture
    {
        public Texture3D(TextureTarget textureTarget, SizedInternalFormat internalFormat)
            : base(textureTarget, internalFormat)
        {
        }
    }
}