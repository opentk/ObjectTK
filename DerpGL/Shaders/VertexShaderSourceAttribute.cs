using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a vertex shader.
    /// </summary>
    public class VertexShaderSourceAttribute
        : ShaderSourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the VertexShaderSourceAttribute.
        /// </summary>
        /// <param name="file"></param>
        public VertexShaderSourceAttribute(string file)
            : base(ShaderType.VertexShader, file)
        {
            
        }
    }
}