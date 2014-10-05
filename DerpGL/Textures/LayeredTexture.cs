using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a layered texture.<br/>
    /// Layered textures are all array and 3D textures.
    /// </summary>
    public abstract class LayeredTexture
        : Texture
    {
        public override bool SupportsLayers { get { return true; } }

        internal LayeredTexture(TextureTarget textureTarget, SizedInternalFormat internalFormat, int levels)
            : base(textureTarget, internalFormat, levels)
        {
        }
    }
}