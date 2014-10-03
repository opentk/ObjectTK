using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies a shader type and the path to its source.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ShaderSourceAttribute
        : Attribute
    {
        /// <summary>
        /// Specifies the type of shader.
        /// </summary>
        public ShaderType Type { get; protected set; }

        /// <summary>
        /// Specifies the path and filename to the source file.
        /// </summary>
        public string File { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the ShaderSourceAttribute.
        /// </summary>
        /// <param name="type">The type of the shader.</param>
        /// <param name="file">The source filename.</param>
        public ShaderSourceAttribute(ShaderType type, string file)
        {
            Type = type;
            File = file;
        }

        /// <summary>
        /// Retrieves all shader sources from attributes tagged to the shader instance given.
        /// </summary>
        /// <param name="shader">The shader of which sources are to be found.</param>
        /// <returns>Mapping of ShaderType and source filename.</returns>
        public static Dictionary<ShaderType, string> GetShaderSources(Shader shader)
        {
            return shader.GetType().GetCustomAttributes<ShaderSourceAttribute>(true).ToDictionary(_ => _.Type, _ => _.File);
        }
    }
}