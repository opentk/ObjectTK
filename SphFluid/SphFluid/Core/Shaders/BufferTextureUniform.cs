using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    //TODO: refactor texture handles to seperate texture class and move BindBuffer(TextureUnit, TextureHandleClass, Vbo) to TextureUniform
    public class BufferTextureUniform
        : TextureUniform
    {
        private readonly int _bufferTexture;

        public BufferTextureUniform(int program, string name)
            : base(program, name)
        {
            _bufferTexture = GL.GenTexture();
        }

        public void BindBuffer<T>(TextureUnit unit, Vbo<T> buffer, SizedInternalFormat format = SizedInternalFormat.R32f)
            where T : struct
        {
            const int zero = (int)TextureUnit.Texture0;
            Set((int)unit - zero);
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureBuffer, _bufferTexture);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, format, buffer.Handle);
        }
    }
}