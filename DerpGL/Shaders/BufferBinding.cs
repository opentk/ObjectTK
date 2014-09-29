using System;
using DerpGL.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public abstract class BufferBinding
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BufferBinding));

        public readonly BufferRangeTarget BindingTarget;
        public readonly int Index;
        public readonly bool Active;
        protected int Binding;

        protected BufferBinding(int program, string name, BufferRangeTarget bindingTarget, ProgramInterface programInterface)
            : base(program, name)
        {
            BindingTarget = bindingTarget;
            Index = GL.GetProgramResourceIndex(program, programInterface, name);
            Active = Index != -1;
            if (!Active) Logger.WarnFormat("Binding block not found or not active: {0}", name);
            Binding = -1;
        }

        public virtual void ChangeBinding(int binding)
        {
            Binding = binding;
        }

        public void BindBuffer<T>(Buffer<T> buffer)
            where T : struct
        {
            if (!Active) return;
            GL.BindBufferBase(BindingTarget, Binding, buffer.Handle);
        }

        public void BindBuffer<T>(Buffer<T> buffer, int offset, int size)
            where T : struct
        {
            if (!Active) return;
            GL.BindBufferRange(BindingTarget, Binding, buffer.Handle, (IntPtr)offset, (IntPtr)size);
        }

        public void Unbind()
        {
            if (!Active) return;
            GL.BindBufferBase(BindingTarget, Binding, 0);
        }
    }
}