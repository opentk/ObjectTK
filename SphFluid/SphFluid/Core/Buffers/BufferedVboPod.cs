using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Shaders;

namespace SphFluid.Core.Buffers
{
    public class BufferedVboPod
        : VboPod
    {
        private int _bufferTexture;

        public BufferedVboPod()
        {
            _bufferTexture = GL.GenTexture();
        }

        public void BindBufferTexture(TextureUnit unit, Uniform<int> uniform)
        {
            const int zero = (int)TextureUnit.Texture0;
            uniform.Set((int)unit - zero);
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.TextureBuffer, _bufferTexture);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.R32f, Ping.Handle);
        }

        public override void Release()
        {
            base.Release();
            GL.DeleteTextures(1, ref _bufferTexture);
        }
    }
}