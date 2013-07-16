using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Vbo
        : IReleasable
    {
        public int Handle
        {
            get { return VboHandle; }
        }

        protected int VboHandle;

        public Vbo()
        {
            GL.GenBuffers(1, out VboHandle);
        }

        public void UploadData<T>(BufferTarget bufferTarget, T[] data, int elementSize, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
            where T : struct
        {
            var fullSize = data.Length * elementSize;
            // upload data to buffer
            GL.BindBuffer(bufferTarget, VboHandle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, data, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
        }

        public void AllocateData(BufferTarget bufferTarget, int fullSize, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
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
        }
    }
}