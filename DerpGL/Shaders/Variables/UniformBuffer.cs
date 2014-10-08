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

namespace DerpGL.Shaders.Variables
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
            PostLink += OnPostLink;
        }

        private void OnPostLink()
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