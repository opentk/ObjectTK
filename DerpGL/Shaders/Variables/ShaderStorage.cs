using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader storage buffer object (SSBO) binding.
    /// </summary>
    public sealed class ShaderStorage
        : BufferBinding
    {
        internal ShaderStorage(int program, string name)
            : base(program, name, BufferRangeTarget.ShaderStorageBuffer, ProgramInterface.ShaderStorageBlock)
        {
            //TODO: find out if the current binding point can be queried, like it can be for uniform blocks
            // set the binding point to the blocks index
            if (Active) ChangeBinding(Index);
        }

        /// <summary>
        /// Assigns a binding point to this shader storage block.
        /// </summary>
        /// <param name="binding"></param>
        public override void ChangeBinding(int binding)
        {
            base.ChangeBinding(binding);
            GL.ShaderStorageBlockBinding(Program, Index, binding);
        }
    }
}