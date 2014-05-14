using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Vbo<T>
        : IReleasable
        where T : struct
    {
        public int Handle { get; private set; }
        public int TextureHandle { get; private set; }

        protected int ElementCount;

        public int ElementSize
        {
            get { return Marshal.SizeOf(typeof (T)); }
        }

#if DEBUG
        /// <summary>
        /// Retrieves data back from vram for debugging purposes.
        /// </summary>
        public T[] Content
        {
            get
            {
                var items = new T[ElementCount];
                GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
                GL.GetBufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(ElementSize * ElementCount), items);
                return items;
            }
        }
#endif
        
        public Vbo()
        {
            Handle = GL.GenBuffer();
            TextureHandle = GL.GenTexture();
        }

        public void Release()
        {
            GL.DeleteBuffer(Handle);
            GL.DeleteTexture(TextureHandle);
        }

        /// <summary>
        /// Allocates buffer memory and uploads given data to it.
        /// </summary>
        /// <param name="bufferTarget"></param>
        /// <param name="data"></param>
        /// <param name="usageHint"></param>
        public void Init(BufferTarget bufferTarget, T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            ElementCount = data.Length;
            var fullSize = data.Length * ElementSize;
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, data, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
        }

        /// <summary>
        /// Allocates buffer memory and initializes it to zero.
        /// </summary>
        /// <param name="bufferTarget"></param>
        /// <param name="elementCount"></param>
        /// <param name="usageHint"></param>
        public void Init(BufferTarget bufferTarget, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            ElementCount = elementCount;
            var fullSize = elementCount * ElementSize;
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, IntPtr.Zero, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
        }

        /// <summary>
        /// Overwrite part of the buffer memory.
        /// </summary>
        /// <param name="bufferTarget"></param>
        /// <param name="elementOffset"></param>
        /// <param name="data"></param>
        public void SubData(BufferTarget bufferTarget, int elementOffset, T[] data)
        {
            var offset = elementOffset * ElementSize;
            var subSize = data.Length * ElementSize;
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferSubData(bufferTarget, (IntPtr)offset, (IntPtr)subSize, data);
        }

        protected void CheckBufferSize(BufferTarget bufferTarget, int size)
        {
            // check if uploaded size is correct
            int uploadedSize;
            GL.GetBufferParameter(bufferTarget, BufferParameterName.BufferSize, out uploadedSize);
            if (uploadedSize != size)
            {
                throw new ApplicationException(string.Format(
                    "Problem uploading data to buffer object. Tried to upload {0} bytes, but uploaded {1}.", size, uploadedSize));
            }
        }
    }
}