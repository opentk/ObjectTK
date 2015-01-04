using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("ExampleShader.Vertex")]
    [FragmentShaderSource("ExampleShader.Fragment")]
    public class ExampleProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}