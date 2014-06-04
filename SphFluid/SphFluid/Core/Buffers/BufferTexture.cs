using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class BufferTexture<T>
        : Buffer<T>
        where T : struct
    {
        /// <summary>
        /// The OpenGL handle to the texture;
        /// </summary>
        public int TextureHandle { get; private set; }

        /// <summary>
        /// The format to be used when accessing this buffer via the buffer texture.
        /// </summary>
        public SizedInternalFormat BufferTextureFormat
        {
            get
            {
                return _bufferTextureFormat;
            }
            set
            {
                _bufferTextureFormat = value;
                BindBufferToTexture();
            }
        }

        private SizedInternalFormat _bufferTextureFormat;

        public BufferTexture()
        {
            TextureHandle = GL.GenTexture();
            // default internal buffer texture format
            _bufferTextureFormat = SizedInternalFormat.R32f;
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            GL.DeleteTexture(TextureHandle);
        }

        protected override void Init(BufferTarget bufferTarget, int elementCount, T[] data, BufferUsageHint usageHint)
        {
            base.Init(bufferTarget, elementCount, data, usageHint);
            BindBufferToTexture();
        }

        protected void BindBufferToTexture()
        {
            if (!Initialized) return;
            GL.BindTexture(TextureTarget.TextureBuffer, TextureHandle);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, BufferTextureFormat, Handle);
        }
    }
}