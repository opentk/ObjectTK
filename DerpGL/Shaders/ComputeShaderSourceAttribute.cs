using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a compute shader.
    /// </summary>
    public class ComputeShaderSourceAttribute
        : ShaderSourceAttribute
    {
        public ComputeShaderSourceAttribute(string file)
            : base(ShaderType.ComputeShader, file)
        {
            
        }
    }
}