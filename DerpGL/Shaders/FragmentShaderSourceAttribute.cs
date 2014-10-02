using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies the source of a fragment shader.
    /// </summary>
    public class FragmentShaderSourceAttribute
        : ShaderSourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FragmentShaderSourceAttribute.
        /// </summary>
        /// <param name="file"></param>
        public FragmentShaderSourceAttribute(string file)
            : base(ShaderType.FragmentShader, file)
        {
            
        }
    }
}