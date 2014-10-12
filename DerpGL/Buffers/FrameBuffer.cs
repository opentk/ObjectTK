#region License
// DerpGL License
// Copyright (C) 2013-2014 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using System;
using DerpGL.Textures;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    /// <summary>
    /// Represents a framebuffer object.
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
        /// Attaches the given texture level to the an attachment point.
        /// </summary>
        /// <remarks>
        /// If texture is a three-dimensional, cube map array, cube map, one- or two-dimensional array, or two-dimensional multisample array texture
        /// the specified texture level is an array of images and the framebuffer attachment is considered to be layered.
        /// </remarks>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferAttachment attachment, Texture texture, int level = 0)
        {
            texture.AssertLevel(level);
            AssertActive();
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, attachment, texture.Handle, level);
            CheckState();
        }

        /// <summary>
        /// Attaches a single layer of the given texture level to an attachment point.
        /// </summary>
        /// <remarks>
        /// Note that for cube maps and cube map arrays the <paramref name="layer"/> parameter actually indexes the layer-faces.<br/>
        /// Thus for cube maps the layer parameter equals the face to be bound.<br/>
        /// For cube map arrays the layer parameter can be calculated as 6 * arrayLayer + face, which is done automatically when using
        /// the corresponding overload <see cref="Attach(OpenTK.Graphics.OpenGL.FramebufferAttachment,DerpGL.Textures.TextureCubemapArray,int,int,int)"/>.
        /// </remarks>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="layer">The layer of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferAttachment attachment, LayeredTexture texture, int layer, int level = 0)
        {
            texture.AssertLevel(level);
            AssertActive();
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, attachment, texture.Handle, level, layer);
            CheckState();
        }

        /// <summary>
        /// Attaches a single face of the given cube map texture level to the an attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="face">The cube map face of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferAttachment attachment, TextureCubemap texture, int face, int level = 0)
        {
            Attach(attachment, (LayeredTexture)texture, face, level);
        }

        /// <summary>
        /// Attaches a single face of the given cube map array texture level to an attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="arrayLayer">The layer of the texture to attach.</param>
        /// <param name="face">The cube map face of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferAttachment attachment, TextureCubemapArray texture, int arrayLayer, int face, int level = 0)
        {
            Attach(attachment, (LayeredTexture)texture, 6 * arrayLayer + face, level);
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