//
// UniformBuffer.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Represents a uniform buffer object (UBO) binding.
    /// </summary>
    public sealed class UniformBuffer
        : BufferBinding
    {
        internal UniformBuffer()
            : base(BufferRangeTarget.UniformBuffer, ProgramInterface.UniformBlock)
        {
        }

        internal override void OnLink()
        {
            base.OnLink();
            // retrieve the default binding point
            if (Active) GL.GetActiveUniformBlock(ProgramHandle, Index, ActiveUniformBlockParameter.UniformBlockBinding, out Binding);
        }

        /// <summary>
        /// Assigns a binding point to this uniform block.
        /// </summary>
        /// <param name="binding">The binding point to assign.</param>
        public override void ChangeBinding(int binding)
        {
            base.ChangeBinding(binding);
            if (!Active) return;
            GL.UniformBlockBinding(ProgramHandle, Index, binding);
        }

        /// <summary>
        /// Retrieves the total size of the uniform block.
        /// </summary>
        /// <returns>The total size of the uniform block.</returns>
        public int GetBlockSize()
        {
            int size;
            GL.GetActiveUniformBlock(ProgramHandle, Index, ActiveUniformBlockParameter.UniformBlockDataSize, out size);
            return size;
        }

        /// <summary>
        /// Retrieves the offsets of the uniforms within the block to the start of the block.
        /// </summary>
        /// <param name="offsets">The offsets of the uniforms within the block.</param>
        public void GetBlockOffsets(out int[] offsets)
        {
            int uniforms;
            GL.GetActiveUniformBlock(ProgramHandle, Index, ActiveUniformBlockParameter.UniformBlockActiveUniforms, out uniforms);
            var indices = new int[uniforms];
            GL.GetActiveUniformBlock(ProgramHandle, Index, ActiveUniformBlockParameter.UniformBlockActiveUniformIndices, indices);
            offsets = new int[uniforms];
            GL.GetActiveUniforms(ProgramHandle, uniforms, indices, ActiveUniformParameter.UniformOffset, offsets);
        }
    }
}