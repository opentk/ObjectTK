using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shaders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ShaderSourceAttribute
        : Attribute
    {
        public string File { get; protected set; }

        public ShaderSourceAttribute(string file)
        {
            File = file;
        }

        /// <summary>
        /// Retrieves all shader sources from attributes tagged to the shader instance given.
        /// </summary>
        /// <param name="shader">The shader of which sources are to be found.</param>
        /// <returns>Mapping of ShaderType and source filename.</returns>
        public static Dictionary<ShaderType, string> GetShaderSources(Shader shader)
        {
            var shaderSources = new Dictionary<ShaderType, string>();
            shader.GetType().GetCustomAttributes<VertexShaderSourceAttribute>(true).ToList().ForEach(_ => shaderSources.Add(ShaderType.VertexShader, _.File));
            shader.GetType().GetCustomAttributes<GeometryShaderSourceAttribute>(true).ToList().ForEach(_ => shaderSources.Add(ShaderType.GeometryShader, _.File));
            shader.GetType().GetCustomAttributes<FragmentShaderSourceAttribute>(true).ToList().ForEach(_ => shaderSources.Add(ShaderType.FragmentShader, _.File));
            return shaderSources;
        }
    }

    public class VertexShaderSourceAttribute : ShaderSourceAttribute { public VertexShaderSourceAttribute(string file) : base(file) { } }
    public class GeometryShaderSourceAttribute : ShaderSourceAttribute { public GeometryShaderSourceAttribute(string file) : base(file) { } }
    public class FragmentShaderSourceAttribute : ShaderSourceAttribute { public FragmentShaderSourceAttribute(string file) : base(file) { } }
}