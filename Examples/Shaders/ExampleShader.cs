using DerpGL.Shaders;
using DerpGL.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("ExampleShader.vs")]
    [FragmentShaderSource("ExampleShader.fs")]
    public class ExampleShader
        : Shader
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}