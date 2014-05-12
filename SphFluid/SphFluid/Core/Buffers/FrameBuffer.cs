using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class FrameBuffer
    {
        public int FramebufferHandle { get { return _fboHandle; } }
        private readonly int _fboHandle;

        public FrameBuffer()
        {
            // create framebuffer
            GL.GenFramebuffers(1, out _fboHandle);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fboHandle);
        }

        private static void CheckState()
        {
            Utility.Assert("Unable to attach texture to framebuffer");
            Utility.Assert(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer), FramebufferErrorCode.FramebufferComplete, "Unable to create framebuffer");
        }

        public void Attach(FramebufferAttachment attachment, Texture texture)
        {
            // attach texture to framebuffer
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, texture.TextureHandle, 0);
            CheckState();
        }
        
        public void AttachLayer(FramebufferAttachment attachment, Texture texture, int layer)
        {
            // attach a layer of the texture to the framebuffer
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, attachment, texture.TextureHandle, 0, layer);
            CheckState();
        }

        public void Detach(FramebufferAttachment attachment)
        {
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, 0, 0);
            CheckState();
        }
    }
}