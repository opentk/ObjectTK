using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Provides methods for creating texture objects in ways not covered by constructors.
    /// </summary>
    public static class TextureFactory
    {
        /// <summary>
        /// Creates a new Texture2D instance using the given texture handle.<br/>
        /// The width, height and internal format are queried from OpenGL and passed to the instance.
        /// The number of mipmap levels can not be queried and must be specified, otherwise it is set to one.
        /// </summary>
        /// <param name="textureHandle">An active handle to a 2D texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        /// <returns>A new Texture2D instance.</returns>
        public static Texture2D AquireTexture2D(int textureHandle, int levels = 1)
        {
            int width, height, internalFormat;
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out height);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureInternalFormat, out internalFormat);
            return new Texture2D(textureHandle, (SizedInternalFormat)internalFormat, width, height, levels);
        }
    }
}