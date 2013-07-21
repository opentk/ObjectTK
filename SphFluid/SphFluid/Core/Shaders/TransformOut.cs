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
            GL.BindBufferBase(BufferTarget.TransformFeedbackBuffer, Index, buffer.Handle);
        }
    }
}