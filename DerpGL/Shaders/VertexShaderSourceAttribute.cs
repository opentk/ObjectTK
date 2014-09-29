using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a vertex shader.
    /// </summary>
    public class VertexShaderSourceAttribute
        : ShaderSourceAttribute
    {
        public VertexShaderSourceAttribute(string file)
            : base(ShaderType.VertexShader, file)
        {
            
        }
    }
}