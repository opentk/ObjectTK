using DerpGL.Textures;
using OpenTK.Graphics.OpenGL4;

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
        /// <param name="layered">Specifies whether a layered texture binding is to be established.</param>
        /// <param name="layer">If <paramref name="layered"/> is false, specifies the layer of the texture to be bound, ignored otherwise.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        /// <param name="format">Specifies the format that the elements of the texture will be treated as.</param>
        public void BindImage(int imageUnit, int textureHandle, bool layered, int layer, TextureAccess access, SizedInternalFormat format)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, textureHandle, 0, layered, layer, access, format);
        }

        /// <summary>
        /// Binds the given texture to an image unit.<br/>
        /// The format argument is taken from the internal format of the texture.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to be bound.</param>
        /// <param name="layered">Specifies whether a layered texture binding is to be established.</param>
        /// <param name="layer">If <paramref name="layered"/> is false, specifies the layer of the texture to be bound, ignored otherwise.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, Texture texture, bool layered, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, layered, layer, access, texture.InternalFormat);
        }

        /// <summary>
        /// Binds the given texture to an image unit.<br/>
        /// The format argument is taken from the internal format of the texture.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to be bound.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, Texture2D texture, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, false, 0, access, texture.InternalFormat);
        }

        /// <summary>
        /// Binds all layers of the given texture array to an image unit.<br/>
        /// The format argument is taken from the internal format of the texture.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture array to be bound.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, Texture2DArray texture, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, true, 0, access, texture.InternalFormat);
        }

        /// <summary>
        /// Binds a single layer of the given texture array to an image unit.<br/>
        /// The format argument is taken from the internal format of the texture.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture array to be bound.</param>
        /// <param name="layer">The layer of the texture array to be bound.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindTexture(int imageUnit, Texture2DArray texture, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, false, layer, access, texture.InternalFormat);
        }

        /// <summary>
        /// Binds the given buffer texture to an image unit.<br/>
        /// The format argument is taken from the internal format of the buffer object.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="buffer">The buffer texture to be bound.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void BindBuffer(int imageUnit, BufferTexture buffer, TextureAccess access)
        {
            BindImage(imageUnit, buffer.Handle, false, 0, access, buffer.InternalFormat);
        }
    }
}