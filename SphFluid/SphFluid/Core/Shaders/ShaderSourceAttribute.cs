using System;

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
    }

    public class VertexShaderSourceAttribute : ShaderSourceAttribute { public VertexShaderSourceAttribute(string file) : base(file) { } }
    public class GeometryShaderSourceAttribute : ShaderSourceAttribute { public GeometryShaderSourceAttribute(string file) : base(file) { } }
    public class FragmentShaderSourceAttribute : ShaderSourceAttribute { public FragmentShaderSourceAttribute(string file) : base(file) { } }
}