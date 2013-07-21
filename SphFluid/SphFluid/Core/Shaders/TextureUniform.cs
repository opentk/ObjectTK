using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    public class TextureUniform
        : Uniform<int>
    {
        public TextureUniform(int program, string name)
            : base(program, name, GL.Uniform1)
        {
        }

        public void BindTexture(TextureTarget target, TextureUnit unit, Texture texture)
        {
            const int zero = (int)TextureUnit.Texture0;
            Set((int)unit - zero);
            GL.ActiveTexture(unit);
            GL.BindTexture(target, texture.TextureHandle);
        }

        public void BindBuffer<T>(TextureUnit unit, Vbo<T> buffer, SizedInternalFormat format = SizedInternalFormat.R32f)
            where T : struct
        {
            const int zero = (int)TextureUnit.Texture0;
            Set((int)unit - zero);
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureBuffer, buffer.TextureHandle);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, format, buffer.Handle);
        }
    }
}