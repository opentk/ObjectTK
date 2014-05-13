using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class RenderBuffer
        : IReleasable
    {
        public int Handle { get; private set; }

        public RenderBuffer()
        {
            Handle = GL.GenRenderbuffer();
        }

        public void Release()
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