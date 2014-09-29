using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a fragment shader.
    /// </summary>
    public class FragmentShaderSourceAttribute
        : ShaderSourceAttribute
    {
        public FragmentShaderSourceAttribute(string file)
            : base(ShaderType.FragmentShader, file)
        {
            
        }
    }
}