using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class ShaderStorage
        : BufferBinding
    {
        public ShaderStorage(int program, string name)
            : base(program, name, BufferRangeTarget.ShaderStorageBuffer, ProgramInterface.ShaderStorageBlock)
        {
            //TODO: find out if the current binding point can be queried, like it can be for uniform blocks
            // set the binding point to the blocks index
            if (Active) ChangeBinding(Index);
        }

        public override void ChangeBinding(int binding)
        {
            base.ChangeBinding(binding);
            GL.ShaderStorageBlockBinding(Program, Index, binding);
        }
    }
}