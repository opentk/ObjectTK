using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    /// <summary>
    /// Represents a transform feedback buffer.
    /// </summary>
    public class TransformFeedbackBuffer
        : GLResource
    {
        /// <summary>
        /// Creates a new transform feedback buffer.
        /// </summary>
        public TransformFeedbackBuffer()
            : base(GL.GenTransformFeedback())
        {
            
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteTransformFeedback(Handle);
        }

        /// <summary>
        /// Binds the transform feedback buffer.
        /// </summary>
        public void Bind()
        {
            GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, Handle);
        }
    }
}