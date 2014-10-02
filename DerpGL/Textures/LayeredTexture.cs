using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a layered texture.<br/>
    /// Layered textures are <see cref="Texture2DArray"/>, <see cref="Texture3D"/>
    /// </summary>
    public abstract class LayeredTexture
        : MipmapTexture
    {
        internal LayeredTexture(TextureTarget textureTarget, SizedInternalFormat internalFormat)
            : base(textureTarget, internalFormat)
        {
        }

        internal LayeredTexture(TextureTarget textureTarget, SizedInternalFormat internalFormat, int levels)
            : base(textureTarget, internalFormat, levels)
        {
        }
    }
}