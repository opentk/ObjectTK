using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class RenderBuffer
        : GLResource
    {
        public RenderBuffer()
            : base(GL.GenRenderbuffer())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteRenderbuffer(Handle);
        }

        public void Init(RenderbufferStorage storage, int width, int height)
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, width, height);
        }
    }
}