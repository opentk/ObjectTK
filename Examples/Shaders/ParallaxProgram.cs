using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("Parallax.Vertex")]
    [FragmentShaderSource("Parallax.Fragment")]
    public class ParallaxProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InTangent { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib InTexCoord { get; protected set; }

        public Uniform<Matrix4> ModelViewMatrix { get; protected set; }
        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public Uniform<Matrix3> NormalMatrix { get; protected set; }

        public Uniform<Vector3> Light_Position { get; protected set; }
        public Uniform<Vector3> Camera_Position { get; protected set; }

        public TextureUniform<Texture2D> Material_DiffuseAndHeight { get; protected set; }
        public TextureUniform<Texture2D> Material_NormalAndGloss { get; protected set; }
        public Uniform<Vector3> Material_ScaleBiasShininess { get; protected set; }

        public Uniform<Vector3> Light_DiffuseColor { get; protected set; }
        public Uniform<Vector3> Light_SpecularColor { get; protected set; }
    }
}