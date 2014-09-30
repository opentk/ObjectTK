using DerpGL.Shaders;
using DerpGL.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.AdvancedExamples
{
    [VertexShaderSource("SimpleColor.vs")]
    [FragmentShaderSource("SimpleColor.fs")]
    public class SimpleColorShader
        : Shader
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(4, VertexAttribPointerType.UnsignedByte, Normalized = true)]
        public VertexAttrib InColor { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}