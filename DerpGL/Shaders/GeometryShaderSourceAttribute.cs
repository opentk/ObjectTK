using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a geometry shader.
    /// </summary>
    public class GeometryShaderSourceAttribute
        : ShaderSourceAttribute
    {
        public GeometryShaderSourceAttribute(string file)
            : base(ShaderType.GeometryShader, file)
        {
            
        }
    }
}