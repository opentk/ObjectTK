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
using DerpGL.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader buffer binding point identified by its resource index.
    /// </summary>
    public abstract class BufferBinding
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BufferBinding));

        /// <summary>
        /// The target to use when binding to this point.
        /// </summary>
        public readonly BufferRangeTarget BindingTarget;

        /// <summary>
        /// The resource index of this binding point.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Current binding point
        /// </summary>
        protected int Binding;

        private readonly ProgramInterface _programInterface;

        internal BufferBinding(BufferRangeTarget bindingTarget, ProgramInterface programInterface)
        {
            BindingTarget = bindingTarget;
            _programInterface = programInterface;
        }

        internal override void OnLink()
        {
            Index = GL.GetProgramResourceIndex(ProgramHandle, _programInterface, Name);
            Active = Index > -1;
            if (!Active) Logger.WarnFormat("Binding block not found or not active: {0}", Name);
            Binding = -1;
        }

        /// <summary>
        /// Assigns a binding point.
        /// </summary>
        public virtual void ChangeBinding(int binding)
        {
            Binding = binding;
        }

        /// <summary>
        /// Binds a buffer to this binding point.
        /// </summary>
        /// <typeparam name="T">The type of the container elements.</typeparam>
        /// <param name="buffer">The buffer to bind.</param>
        public void BindBuffer<T>(Buffer<T> buffer)
            where T : struct
        {
            if (!Active) return;
            GL.BindBufferBase(BindingTarget, Binding, buffer.Handle);
        }

        /// <summary>
        /// Binds a buffer to this binding point.
        /// </summary>
        /// <typeparam name="T">The type of the container elements.</typeparam>
        /// <param name="buffer">The buffer to bind.</param>
        /// <param name="offset">The starting offset in basic machine units into the buffer object buffer. </param>
        /// <param name="size">The amount of data in machine units that can be read from the buffer object while used as an indexed target. </param>
        public void BindBuffer<T>(Buffer<T> buffer, int offset, int size)
            where T : struct
        {
            if (!Active) return;
            GL.BindBufferRange(BindingTarget, Binding, buffer.Handle, (IntPtr)offset, (IntPtr)size);
        }

        /// <summary>
        /// Unbinds any buffer from this binding point.
        /// </summary>
        public void Unbind()
        {
            if (!Active) return;
            GL.BindBufferBase(BindingTarget, Binding, 0);
        }
    }
}