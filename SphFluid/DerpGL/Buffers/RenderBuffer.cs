using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class RenderBuffer
        : ContextResource
    {
        public int Handle { get; private set; }

        public RenderBuffer()
        {
            Handle = GL.GenRenderbuffer();
        }

        protected override void OnRelease()
        {
            GL.DeleteRenderbuffer(Handle);
        }

        public void Init(RenderbufferStorage storage, int width, int height)
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, width, height);
        }
    }
}