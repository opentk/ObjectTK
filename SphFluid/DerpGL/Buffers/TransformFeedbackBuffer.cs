using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class TransformFeedbackBuffer
        : GLResource
    {
        public TransformFeedbackBuffer()
            : base(GL.GenTransformFeedback())
        {
            
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteTransformFeedback(Handle);
        }

        public void Bind()
        {
            GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, Handle);
        }
    }
}