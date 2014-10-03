using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a compute shader.
    /// </summary>
    public class ComputeShaderSourceAttribute
        : ShaderSourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the ComputeShaderSourceAttribute.
        /// </summary>
        /// <param name="file"></param>
        public ComputeShaderSourceAttribute(string file)
            : base(ShaderType.ComputeShader, file)
        {
            
        }
    }
}