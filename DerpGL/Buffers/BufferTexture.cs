using System;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class BufferTexture<T>
        : Texture
        where T : struct
    {
        public BufferTexture(SizedInternalFormat internalFormat)
            : base(internalFormat)
        {
        }

        public BufferTexture()
            : this(SizedInternalFormat.R32f)
        {
        }

        public void BindBufferToTexture(Buffer<T> buffer)
        {
            BindBufferToTexture(buffer, InternalFormat);
        }

        public void BindBufferToTexture(Buffer<T> buffer, SizedInternalFormat internalFormat)
        {
            if (!buffer.Initialized) throw new ApplicationException("Can not bind uninitialized buffer to buffer texture.");
            InternalFormat = internalFormat;
            GL.BindTexture(TextureTarget.TextureBuffer, Handle);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, InternalFormat, Handle);
        }
    }
}