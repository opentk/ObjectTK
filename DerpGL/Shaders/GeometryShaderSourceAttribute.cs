using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a geometry shader.
    /// </summary>
    public class GeometryShaderSourceAttribute
        : ShaderSourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the GeometryShaderSourceAttribute.
        /// </summary>
        /// <param name="file"></param>
        public GeometryShaderSourceAttribute(string file)
            : base(ShaderType.GeometryShader, file)
        {
            
        }
    }
}