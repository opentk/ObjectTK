using System;
using DerpGL.Core.Buffers;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Core.Shaders
{
    public class ShaderStorage
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ShaderStorage));

        public readonly int Index;

        public ShaderStorage(int program, string name)
            : base(program, name)
        {
            Index = GL.GetProgramResourceIndex(program, ProgramInterface.ShaderStorageBlock, name);
            if (Index == (int)ArbUniformBufferObject.InvalidIndex) Logger.WarnFormat("Shader storage buffer binding not found or not active: {0}", name);
            // set the binding point to the blocks index
            ChangeBinding(Index);
        }

        public void ChangeBinding(int binding)
        {
            //TODO: find out if the current binding point can also be queried
            GL.ShaderStorageBlockBinding(Program, Index, binding);
        }

        public void BindBuffer<T>(Buffer<T> buffer)
            where T : struct
        {
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, Index, buffer.Handle);
        }

        public void BindBuffer<T>(Buffer<T> buffer, int offset, int size)
            where T : struct
        {
            GL.BindBufferRange(BufferRangeTarget.ShaderStorageBuffer, Index, buffer.Handle, (IntPtr)offset, (IntPtr)size);
        }

        public void Unbind()
        {
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, Index, 0);
        }
    }
}