using System;
using DerpGL.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL4;

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
        public readonly int Index;

        /// <summary>
        /// Specifies whether this binding point is active in the current shader program.
        /// </summary>
        public readonly bool Active;

        /// <summary>
        /// Current binding point
        /// </summary>
        protected int Binding;

        internal BufferBinding(int program, string name, BufferRangeTarget bindingTarget, ProgramInterface programInterface)
            : base(program, name)
        {
            BindingTarget = bindingTarget;
            Index = GL.GetProgramResourceIndex(program, programInterface, name);
            Active = Index > -1;
            if (!Active) Logger.WarnFormat("Binding block not found or not active: {0}", name);
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