using OpenTK.Graphics.OpenGL;

namespace DerpGL.Core.Buffers
{
    public class TransformFeedbackBuffer
        : ContextResource
    {
        public int Handle { get; private set; }

        public TransformFeedbackBuffer()
        {
            Handle = GL.GenTransformFeedback();
        }

        protected override void OnRelease()
        {
            GL.DeleteTransformFeedback(Handle);
        }

        public void Bind()
        {
            GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, Handle);
        }
    }
}