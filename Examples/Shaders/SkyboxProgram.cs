using DerpGL.Shaders;
using DerpGL.Shaders.Variables;
using DerpGL.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("Skybox.vs")]
    [FragmentShaderSource("Skybox.fs")]
    public class SkyboxProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public TextureUniform<TextureCubemap> Texture { get; protected set; }
    }
}