using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a layered texture.<br/>
    /// Layered textures are <see cref="Texture2DArray"/>, <see cref="Texture3D"/>
    /// </summary>
    public class LayeredTexture
        : Texture
    {
        internal LayeredTexture(TextureTarget textureTarget, SizedInternalFormat internalFormat)
            : base(textureTarget, internalFormat)
        {
        }
    }
}