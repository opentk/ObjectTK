using DerpGL.Buffers;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class ImageUniform
        : Uniform<int>
    {
        public ImageUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        //TODO: refactor somehow to automatically increment the used imageUnit, and reset that counter on Shader.Use()
        // preventing bugs from accidentally using the same imageUnit multiple times
        // the same may apply to textures
        public void BindImage(int imageUnit, int textureHandle, bool layered, int layer, TextureAccess access, SizedInternalFormat format)
        {
            if (!Set(imageUnit)) return;
            GL.BindImageTexture(imageUnit, textureHandle, 0, layered, layer, access, format);
        }

        public void BindTexture(int imageUnit, Texture texture, bool layered, int layer, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, layered, layer, access, texture.InternalFormat);
        }

        public void BindTexture(int imageUnit, Texture2D texture, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, false, 0, access, texture.InternalFormat);
        }

        public void BindTexture(int imageUnit, Texture2DArray texture, TextureAccess access)
        {
            BindImage(imageUnit, texture.Handle, true, 0, access, texture.InternalFormat);
        }

        public void BindBuffer<T>(int imageUnit, BufferTexture<T> buffer, TextureAccess access)
            where T : struct
        {
            BindImage(imageUnit, buffer.Handle, false, 0, access, buffer.InternalFormat);
        }
    }
}