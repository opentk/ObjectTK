using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("TextureGrid.Vertex")]
    [GeometryShaderSource("TextureGrid.Geometry")]
    [FragmentShaderSource("TextureGrid.Fragment")]
    public class TextureGridProgram
        : Program
    {
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(1, VertexAttribPointerType.Float)]
        public VertexAttrib InTexture { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public TextureUniform<Texture2DArray> TextureData { get; protected set; }
    }
}