using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class FrameBuffer
        : IReleasable
    {
        public int FramebufferHandle { get; private set; }

        public FrameBuffer()
        {
            // create framebuffer
            FramebufferHandle = GL.GenFramebuffer();
        }

        public void Release()
        {
            GL.DeleteFramebuffer(FramebufferHandle);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FramebufferHandle);
        }

        private static void CheckState()
        {
#if DEBUG
            Utility.Assert("Unable to attach texture to framebuffer");
            Utility.Assert(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer), FramebufferErrorCode.FramebufferComplete, "Unable to create framebuffer");
#endif
        }

        public void Attach(FramebufferAttachment attachment, Texture texture)
        {
            // attach texture
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, texture.TextureHandle, 0);
            CheckState();
        }

        public void AttachLayer(FramebufferAttachment attachment, Texture texture, int layer)
        {
            // attach layer of texture
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, attachment, texture.TextureHandle, 0, layer);
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
    }
}