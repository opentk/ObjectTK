using DerpGL.Textures;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents an image uniform.
    /// </summary>
    public sealed class ImageUniform
        : Uniform<int>
    {
        internal ImageUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        /// <summary>
        /// Binds a single level of a texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="textureHandle">The handle of the texture.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="layered">Specifies whether a layered texture binding is to be established.</param>
        /// <param name="layer">If <paramref name="layered"/> is false, specifies the layer of the texture to be bound, ignored otherwise.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        /// <param name="format">Specifies the format that the elements of the texture will be treated as.</param>
        public void BindImage(int imageUnit, int textureHandle, int level, bool layered, int layer, TextureAccess access, SizedInternalFormat format)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, textureHandle, level, layered, layer, access, format);
        }

        /// <summary>
        /// Binds a single level of a texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="layered">Specifies whether a layered texture binding is to be established.</param>
        /// <param name="layer">If <paramref name="layered"/> is false, specifies the layer of the texture to be bound, ignored otherwise.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindImage(int imageUnit, Texture texture, int level, bool layered, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, level, layered, layer, access, texture.InternalFormat);
        }

        /// <summary>
        /// Binds a single level of a texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, MipmapTexture texture, int level, TextureAccess access)
        {
            BindImage(imageUnit, texture, level, false, 0, access);
        }

        /// <summary>
        /// Binds a single layer of the given texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="layer">The layer of the texture to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, LayeredTexture texture, int level, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture, level, false, layer, access);
        }

        /// <summary>
        /// Binds all layers of the given texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture array to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, LayeredTexture texture, int level, TextureAccess access)
        {
            BindImage(imageUnit, texture, level, true, 0, access);
        }

        /// <summary>
        /// Binds the given buffer texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="buffer">The buffer texture to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindBuffer(int imageUnit, BufferTexture buffer, TextureAccess access)
        {
            BindImage(imageUnit, buffer, 0, false, 0, access);
        }
    }
}