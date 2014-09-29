using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ShaderSourceAttribute
        : Attribute
    {
        public ShaderType Type { get; protected set; }
        public string File { get; protected set; }

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

    public class VertexShaderSourceAttribute : ShaderSourceAttribute { public VertexShaderSourceAttribute(string file) : base(ShaderType.VertexShader, file) { } }
    public class GeometryShaderSourceAttribute : ShaderSourceAttribute { public GeometryShaderSourceAttribute(string file) : base(ShaderType.GeometryShader, file) { } }
    public class FragmentShaderSourceAttribute : ShaderSourceAttribute { public FragmentShaderSourceAttribute(string file) : base(ShaderType.FragmentShader, file) { } }
    public class ComputeShaderSourceAttribute : ShaderSourceAttribute { public ComputeShaderSourceAttribute(string file) : base(ShaderType.ComputeShader, file) { } }
}