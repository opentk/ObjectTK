using System;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shaders
{
    public class TransformOut
    {
        public readonly int Index;

        public TransformOut(int index)
        {
            Index = index;
        }

        public void BindBuffer<T>(Vbo<T> buffer)
            where T : struct
        {
            GL.BindBufferBase(BufferRangeTarget.TransformFeedbackBuffer, Index, buffer.Handle);
        }

        public void BindBuffer<T>(Vbo<T> buffer, int offset, int size)
            where T : struct
        {
            GL.BindBufferRange(BufferRangeTarget.TransformFeedbackBuffer, Index, buffer.Handle, (IntPtr)offset, (IntPtr)size);
        }
    }
}