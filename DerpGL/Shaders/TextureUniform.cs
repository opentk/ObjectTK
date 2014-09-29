using DerpGL.Buffers;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class TextureUniform
        : Uniform<int>
    {
        public TextureUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        protected void BindTexture(TextureTarget target, TextureUnit unit, int textureHandle)
        {
            const int zero = (int)TextureUnit.Texture0;
            if (!Set((int)unit - zero)) return;
            GL.ActiveTexture(unit);
            GL.BindTexture(target, textureHandle);
        }

        public void BindTexture(TextureTarget target, TextureUnit unit, Texture texture)
        {
            BindTexture(target, unit, texture.Handle);
        }

        public void BindBuffer<T>(TextureUnit unit, BufferTexture<T> buffer)
            where T : struct
        {
            BindTexture(TextureTarget.TextureBuffer, unit, buffer.Handle);
        }
    }
}