using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("SimpleTexture.Vertex")]
    [FragmentShaderSource("SimpleTexture.Fragment")]
    public class SimpleTextureProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib InTexCoord { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }

        public TextureUniform<Texture2D> Texture { get; protected set; }
        public Uniform<bool> RenderTexCoords { get; protected set; }
    }
}