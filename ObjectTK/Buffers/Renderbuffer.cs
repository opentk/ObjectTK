//
// Renderbuffer.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Buffers
{
    /// <summary>
    /// Represents a renderbuffer object.
    /// </summary>
    public class Renderbuffer
        : GLObject
    {
        /// <summary>
        /// Creates a new renderbuffer object.
        /// </summary>
        public Renderbuffer()
            : base(GL.GenRenderbuffer())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteRenderbuffer(Handle);
        }

        /// <summary>
        /// Initializes the renderbuffer with the given parameters.
        /// </summary>
        /// <param name="storage">Specifies the internal format.</param>
        /// <param name="width">The width of the renderbuffer.</param>
        /// <param name="height">The height of the renderbuffer.</param>
        public void Init(RenderbufferStorage storage, int width, int height)
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, width, height);
        }
    }
}