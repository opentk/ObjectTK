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
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
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