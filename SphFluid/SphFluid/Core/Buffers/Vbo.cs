using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Vbo<T>
        : IReleasable
        where T : struct
    {
        public int Handle
        {
            get { return VboHandle; }
        }

        public int TextureHandle { get; protected set; }

        protected int VboHandle;
        protected int ElementCount;

        public int ElementSize
        {
            get { return Marshal.SizeOf(typeof (T)); }
        }

#if DEBUG
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
            GL.GenBuffers(1, out VboHandle);
            TextureHandle = GL.GenTexture();
        }

        public void Init(BufferTarget bufferTarget, T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            ElementCount = data.Length;
            var fullSize = data.Length * ElementSize;
            // upload data to buffer
            GL.BindBuffer(bufferTarget, VboHandle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, data, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
        }

        public void Init(BufferTarget bufferTarget, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            ElementCount = elementCount;
            var fullSize = elementCount * ElementSize;
            // upload data to buffer
            GL.BindBuffer(bufferTarget, VboHandle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, IntPtr.Zero, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
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

        public void Release()
        {
            GL.DeleteBuffers(1, ref VboHandle);
            GL.DeleteTexture(TextureHandle);
        }
    }
}