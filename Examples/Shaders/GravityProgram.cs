using ObjectTK.Shaders;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.Shaders
{
    [VertexShaderSource("Gravity.Vertex")]
    [FragmentShaderSource("Gravity.Fragment")]
    public class GravityProgram
        : TransformProgram
    {
        public GravityProgram()
        {
            // define transform feedback varyings and add padding to achieve 16-byte alignment with 3-component vectors
            FeedbackVaryings(TransformFeedbackMode.InterleavedAttribs, OutPosition, SkipComponents1, OutVelocity, SkipComponents1);
        }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InVelocity { get; protected set; }

        public TransformOut OutPosition { get; protected set; }
        public TransformOut OutVelocity { get; protected set; }

        public Uniform<float> CenterMass { get; protected set; }
        public Uniform<float> TimeStep { get; protected set; }
        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}