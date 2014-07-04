using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class FrameBuffer
        : GLResource
    {
        public FrameBuffer()
            : base(GL.GenFramebuffer())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteFramebuffer(Handle);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
        }

        public void Attach(FramebufferAttachment attachment, Texture texture)
        {
            // attach texture
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, texture.Handle, 0);
            CheckState();
        }

        public void AttachLayer(FramebufferAttachment attachment, Texture texture, int layer)
        {
            // attach layer of texture
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, attachment, texture.Handle, 0, layer);
            CheckState();
        }

        public void Attach(FramebufferAttachment attachment, RenderBuffer renderbuffer)
        {
            // attach render buffer
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.Handle);
        }

        public void DetachTexture(FramebufferAttachment attachment)
        {
            // detach texture
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, 0, 0);
            CheckState();
        }

        public void DetachRenderbuffer(FramebufferAttachment attachment)
        {
            // detach render buffer
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, 0);
        }

        private static void CheckState()
        {
#if DEBUG
            Utility.Assert("Unable to attach texture to framebuffer");
            Utility.Assert(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer), FramebufferErrorCode.FramebufferComplete, "Unable to create framebuffer");
#endif
        }
    }
}