using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a layered texture.<br/>
    /// Layered textures are all array, cube map and 3D textures.
    /// </summary>
    public abstract class LayeredTexture
        : Texture
    {
        public override bool SupportsLayers { get { return true; } }

        internal LayeredTexture(SizedInternalFormat internalFormat, int levels)
            : base(internalFormat, levels)
        {
        }
    }
}