using System;
using DerpGL.Textures;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    /// <summary>
    /// Represents an framebuffer object.
    /// </summary>
    public class FrameBuffer
        : GLResource
    {
        /// <summary>
        /// Creates a new framebuffer object.
        /// </summary>
        public FrameBuffer()
            : base(GL.GenFramebuffer())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteFramebuffer(Handle);
        }

        /// <summary>
        /// Handle of the currently active framebuffer.
        /// </summary>
        public static int ActiveFramebufferHandle { get; protected set; }

        /// <summary>
        /// Binds this framebuffer.
        /// </summary>
        public void Bind()
        {
            // remember handle of active framebuffer
            ActiveFramebufferHandle = Handle;
            // bind this framebuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
        }

        /// <summary>
        /// Unbind this framebuffer, i.e. bind the default framebuffer.
        /// </summary>
        public void Unbind()
        {
            ActiveFramebufferHandle = 0;
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if this framebuffer is not the currently active one.
        /// </summary>
        private void AssertActive()
        {
            if (ActiveFramebufferHandle != Handle) throw new InvalidOperationException("Can not access an unbound framebuffer. Call FrameBuffer.Bind() first.");
        }

        /// <summary>
        /// Attaches a texture to the given attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferAttachment attachment, Texture texture, int level = 0)
        {
            AssertActive();
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, texture.Handle, level);
            CheckState();
        }

        /// <summary>
        /// Attaches a single layer of a texture to the given attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="layer">The layer of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void AttachLayer(FramebufferAttachment attachment, LayeredTexture texture, int layer, int level = 0)
        {
            AssertActive();
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, attachment, texture.Handle, level, layer);
            CheckState();
        }

        /// <summary>
        /// Attaches the render buffer to the given attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="renderbuffer">Render buffer to attach.</param>
        public void Attach(FramebufferAttachment attachment, RenderBuffer renderbuffer)
        {
            AssertActive();
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.Handle);
            CheckState();
        }

        /// <summary>
        /// Detaches the currently attached texture from the given attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to detach from.</param>
        public void DetachTexture(FramebufferAttachment attachment)
        {
            AssertActive();
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, 0, 0);
            CheckState();
        }

        /// <summary>
        /// Detaches the currently attached render buffer from the given attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to detach from.</param>
        public void DetachRenderbuffer(FramebufferAttachment attachment)
        {
            AssertActive();
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, 0);
            CheckState();
        }

        private static void CheckState()
        {
#if DEBUG
            Utility.Assert("Error on framebuffer attach/detach");
            Utility.Assert(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer), FramebufferErrorCode.FramebufferComplete, "Framebuffer is not framebuffer complete.");
#endif
        }
    }
}