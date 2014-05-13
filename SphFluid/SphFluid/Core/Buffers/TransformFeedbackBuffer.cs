using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class TransformFeedbackBuffer
        : IReleasable
    {
        public int Handle { get; private set; }

        public TransformFeedbackBuffer()
        {
            Handle = GL.GenTransformFeedback();
        }

        public void Release()
        {
            GL.DeleteTransformFeedback(Handle);
        }

        public void Bind()
        {
            GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, Handle);
        }
    }
}