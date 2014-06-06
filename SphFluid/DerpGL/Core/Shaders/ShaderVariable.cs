namespace DerpGL.Core.Shaders
{
    public class ShaderVariable
    {
        public readonly int Program;
        public readonly string Name;

        protected ShaderVariable(int program, string name)
        {
            Program = program;
            Name = name;
        }
    }
}