using System;
using DerpGL.Core.Buffers;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Core.Shaders
{
    //TODO: Implemented in a completely strange way ..
    public class TransformOut
    {
        public readonly int Index;

        public TransformOut(int index)
        {
            Index = index;
        }

        public void BindBuffer<T>(Buffer<T> buffer)
            where T : struct
        {
            GL.BindBufferBase(BufferRangeTarget.TransformFeedbackBuffer, Index, buffer.Handle);
        }

        public void BindBuffer<T>(Buffer<T> buffer, int offset, int size)
            where T : struct
        {
            GL.BindBufferRange(BufferRangeTarget.TransformFeedbackBuffer, Index, buffer.Handle, (IntPtr)offset, (IntPtr)size);
        }
    }
}