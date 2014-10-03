using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a uniform buffer object (UBO) binding.
    /// </summary>
    public sealed class UniformBuffer
        : BufferBinding
    {
        internal UniformBuffer(int program, string name)
            : base(program, name, BufferRangeTarget.UniformBuffer, ProgramInterface.UniformBlock)
        {
            // retrieve the default binding point
            if (Active) GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockBinding, out Binding);
        }

        /// <summary>
        /// Assigns a binding point to this uniform block.
        /// </summary>
        /// <param name="binding">The binding point to assign.</param>
        public override void ChangeBinding(int binding)
        {
            base.ChangeBinding(binding);
            if (!Active) return;
            GL.UniformBlockBinding(Program, Index, binding);
        }

        /// <summary>
        /// Retrieves the total size of the uniform block.
        /// </summary>
        /// <returns>The total size of the uniform block.</returns>
        public int GetBlockSize()
        {
            int size;
            GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockDataSize, out size);
            return size;
        }

        /// <summary>
        /// Retrieves the offsets of the uniforms within the block to the start of the block.
        /// </summary>
        /// <param name="offsets">The offsets of the uniforms within the block.</param>
        public void GetBlockOffsets(out int[] offsets)
        {
            int uniforms;
            GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockActiveUniforms, out uniforms);
            var indices = new int[uniforms];
            GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockActiveUniformIndices, indices);
            offsets = new int[uniforms];
            GL.GetActiveUniforms(Program, uniforms, indices, ActiveUniformParameter.UniformOffset, offsets);
        }
    }
}