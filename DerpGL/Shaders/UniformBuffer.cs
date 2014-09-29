using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class UniformBuffer
        : BufferBinding
    {
        public UniformBuffer(int program, string name)
            : base(program, name, BufferRangeTarget.UniformBuffer, ProgramInterface.UniformBlock)
        {
            // retrieve the default binding point
            if (Active) GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockBinding, out Binding);

            int size;
            GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockDataSize, out size);
            int uniforms;
            GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockActiveUniforms, out uniforms);
            var indices = new int[uniforms];
            GL.GetActiveUniformBlock(Program, Index, ActiveUniformBlockParameter.UniformBlockActiveUniformIndices, indices);

            var offsets = new int[uniforms];
            GL.GetActiveUniforms(Program, uniforms, indices, ActiveUniformParameter.UniformOffset, offsets);
        }

        public override void ChangeBinding(int binding)
        {
            base.ChangeBinding(binding);
            GL.UniformBlockBinding(Program, Index, binding);
        }
    }
}