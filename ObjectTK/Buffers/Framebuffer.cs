//
// Framebuffer.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using ObjectTK.Exceptions;
using ObjectTK.Textures;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Buffers
{
    /// <summary>
    /// Represents a framebuffer object.
    /// </summary>
    public class Framebuffer
        : GLObject
    {
        /// <summary>
        /// Creates a new framebuffer object.
        /// </summary>
        public Framebuffer()
            : base(GL.GenFramebuffer())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteFramebuffer(Handle);
        }

        /// <summary>
        /// Binds this framebuffer.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        public void Bind(FramebufferTarget target)
        {
            GL.BindFramebuffer(target, Handle);
        }

        /// <summary>
        /// Unbind this framebuffer, i.e. bind the default framebuffer.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        public static void Unbind(FramebufferTarget target)
        {
            GL.BindFramebuffer(target, 0);
        }

        /// <summary>
        /// Attaches the given texture level to the an attachment point.
        /// </summary>
        /// <remarks>
        /// If texture is a three-dimensional, cube map array, cube map, one- or two-dimensional array, or two-dimensional multisample array texture
        /// the specified texture level is an array of images and the framebuffer attachment is considered to be layered.
        /// </remarks>
        /// <param name="target">The framebuffer target to bind to.</param>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferTarget target, FramebufferAttachment attachment, Texture texture, int level = 0)
        {
            texture.AssertLevel(level);
            AssertActive(target);
            GL.FramebufferTexture(target, attachment, texture.Handle, level);
            CheckState(target);
        }

        /// <summary>
        /// Attaches a single layer of the given texture level to an attachment point.
        /// </summary>
        /// <remarks>
        /// Note that for cube maps and cube map arrays the <paramref name="layer"/> parameter actually indexes the layer-faces.<br/>
        /// Thus for cube maps the layer parameter equals the face to be bound.<br/>
        /// For cube map arrays the layer parameter can be calculated as 6 * arrayLayer + face, which is done automatically when using
        /// the corresponding overload <see cref="Attach(FramebufferTarget, FramebufferAttachment, TextureCubemapArray, int, int, int)"/>.
        /// </remarks>
        /// <param name="target">The framebuffer target to bind to.</param>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="layer">The layer of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferTarget target, FramebufferAttachment attachment, LayeredTexture texture, int layer, int level = 0)
        {
            texture.AssertLevel(level);
            AssertActive(target);
            GL.FramebufferTextureLayer(target, attachment, texture.Handle, level, layer);
            CheckState(target);
        }

        /// <summary>
        /// Attaches a single face of the given cube map texture level to the an attachment point.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="face">The cube map face of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferTarget target, FramebufferAttachment attachment, TextureCubemap texture, int face, int level = 0)
        {
            Attach(target, attachment, (LayeredTexture)texture, face, level);
        }

        /// <summary>
        /// Attaches a single face of the given cube map array texture level to an attachment point.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="texture">The texture to attach.</param>
        /// <param name="arrayLayer">The layer of the texture to attach.</param>
        /// <param name="face">The cube map face of the texture to attach.</param>
        /// <param name="level">The level of the texture to attach.</param>
        public void Attach(FramebufferTarget target, FramebufferAttachment attachment, TextureCubemapArray texture, int arrayLayer, int face, int level = 0)
        {
            Attach(target, attachment, (LayeredTexture)texture, 6 * arrayLayer + face, level);
        }

        /// <summary>
        /// Attaches the render buffer to the given attachment point.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        /// <param name="attachment">The attachment point to attach to.</param>
        /// <param name="renderbuffer">Render buffer to attach.</param>
        public void Attach(FramebufferTarget target, FramebufferAttachment attachment, Renderbuffer renderbuffer)
        {
            AssertActive(target);
            GL.FramebufferRenderbuffer(target, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.Handle);
            CheckState(target);
        }

        /// <summary>
        /// Detaches the currently attached texture from the given attachment point.
        /// </summary>
        /// <param name="attachment">The attachment point to detach from.</param>
        /// <param name="target">The framebuffer target to bind to.</param>
        public void DetachTexture(FramebufferTarget target, FramebufferAttachment attachment)
        {
            AssertActive(target);
            GL.FramebufferTexture(target, attachment, 0, 0);
            CheckState(target);
        }

        /// <summary>
        /// Detaches the currently attached render buffer from the given attachment point.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        /// <param name="attachment">The attachment point to detach from.</param>
        public void DetachRenderbuffer(FramebufferTarget target, FramebufferAttachment attachment)
        {
            AssertActive(target);
            GL.FramebufferRenderbuffer(target, attachment, RenderbufferTarget.Renderbuffer, 0);
            CheckState(target);
        }

        /// <summary>
        /// Check if the current framebuffer status is "frambuffer complete", throws on error.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        public void CheckState(FramebufferTarget target)
        {
#if DEBUG
            Utility.Assert("Error on framebuffer attach/detach");
            Utility.Assert(GL.CheckFramebufferStatus(target), FramebufferErrorCode.FramebufferComplete, "Framebuffer is not framebuffer complete.");
#endif
        }

        /// <summary>
        /// Throws an <see cref="ObjectNotBoundException"/> if this framebuffer is not the currently active one.
        /// </summary>
        /// <param name="target">The framebuffer target to bind to.</param>
        public void AssertActive(FramebufferTarget target)
        {
#if DEBUG
            int activeHandle;
            GetPName binding;
            switch (target)
            {
                case FramebufferTarget.ReadFramebuffer: binding = GetPName.ReadFramebufferBinding; break;
                case FramebufferTarget.DrawFramebuffer: binding = GetPName.DrawFramebufferBinding; break;
                case FramebufferTarget.Framebuffer: binding = GetPName.FramebufferBinding; break;
                default: throw new ArgumentOutOfRangeException();
            }
            GL.GetInteger(binding, out activeHandle);
            if (activeHandle != Handle) throw new ObjectNotBoundException("Can not access an unbound framebuffer. Call Framebuffer.Bind() first.");
#endif
        }
    }
}