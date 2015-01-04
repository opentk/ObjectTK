using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("Skybox.Vertex")]
    [FragmentShaderSource("Skybox.Fragment")]
    public class SkyboxProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public TextureUniform<TextureCubemap> Texture { get; protected set; }
    }
}