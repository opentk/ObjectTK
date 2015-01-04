using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Examples.AdvancedExamples
{
    [ExampleProject("Transform feedback with gravity simulation")]
    public class FeedbackGravityExample
        : ExampleWindow
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Particle
        {
            public Vector3 Position;
            private float _padding0;
            public Vector3 Velocity;
            private float _padding1;
        }

        private readonly Random _random;
        private GravityProgram _program;
        private VertexArray _vao;
        private BufferPod<Particle> _buffers;
        private TransformFeedback _feedback;

        // simulation parameters
        private float _centerMass = 0.8309f; //0.65f;
        private int _particleCount = 64000;

        public FeedbackGravityExample()
        {
            _random = new Random();
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
            Keyboard.KeyDown += OnKeyDown;
        }

        private float Rand(float range)
        {
            return (float)(_random.NextDouble() * 2 * range - range);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize shader (load sources, create/compile/link shader program, error checking)
            // when using the factory method the shader sources are retrieved from the ShaderSourceAttributes
            _program = ProgramFactory.Create<GravityProgram>();
            // this program will be used all the time so just activate it once and for all
            _program.Use();

            // create and bind a vertex array
            _vao = new VertexArray();
            _vao.Bind();

            // create and bind transform feedback object
            _feedback = new TransformFeedback();
            _feedback.Bind();
            
            // Writing to a buffer while reading from it is not allowed
            // so we need two buffer objects here, which can be achieved by using the BufferPod<T> type.
            // It contains two buffers, Ping and Pong, to simplify this process.
            _buffers = new BufferPod<Particle>();
            InitializeParticles(_particleCount);

            // enable point sprite rendering
            GL.Enable(EnableCap.PointSprite);
            // enable modification of the point sprite size from the program (vertex shader in this case)
            GL.Enable(EnableCap.ProgramPointSize);
            // enable depth testing
            GL.Enable(EnableCap.DepthTest);
            // set a nice clear color
            GL.ClearColor(Color.Black);

            // set a nice camera angle
            Camera.DefaultState.Position = new Vector3(0,2,-8);
            Camera.ResetToDefault();
        }

        private void InitializeParticles(int n)
        {
            var particles = new Particle[n];
            for (var i = 0; i < particles.Length; i++)
            {
                // generate a flat random cube
                particles[i].Position.X = Rand(0.2f);
                particles[i].Position.Y = Rand(0.02f);
                particles[i].Position.Z = Rand(0.2f);
                // move particles outwards
                particles[i].Position += particles[i].Position.Normalized()*0.8f;
                // calculate velocity perpendicular to the direction towards the gravity center and the y-axis
                particles[i].Velocity = Vector3.Cross(particles[i].Position, -Vector3.UnitY);
            }
            // upload data into the Ping buffer and initialize the pong buffer to the same size
            _buffers.Init(BufferTarget.ArrayBuffer, particles);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            // clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // set up modelview and perspective matrix
            SetupPerspective();

            // set uniforms
            _program.CenterMass.Set(_centerMass);
            _program.TimeStep.Set((float)e.Time);
            _program.ModelViewProjectionMatrix.Set(ModelView * Projection);

            // set up binding of the shader variable to the buffer object
            _vao.BindAttribute(_program.InPosition, _buffers.Ping);
            _vao.BindAttribute(_program.InVelocity, _buffers.Ping, 16);
            
            // set target buffer for transform feedback
            // the two outputs are interleaved into the same buffer
            // so we only have to bind one of them as both binding indices are equal:
            // _program.OutPosition.Index == _program.OutVelocity.Index
            // when you choose to write into different buffers you need to bind all of them
            _feedback.BindOutput(_program.OutPosition, _buffers.Pong);
            
            // render the buffer and capture transform feedback outputs
            _feedback.Begin(TransformFeedbackPrimitiveType.Points);
            _vao.DrawArrays(PrimitiveType.Points, 0, _buffers.Ping.ElementCount);
            _feedback.End();

            // swap all the buffers!
            _buffers.Swap();
            SwapBuffers();
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            const float up = 1.1f;
            const float down = 0.9f;
            var reinitialize = false;
            switch (e.Key)
            {
                case Key.Up: _centerMass *= up; break;
                case Key.Down: _centerMass *= down; break;
                case Key.Space:
                case Key.Right:
                case Key.Left:
                    if (e.Key == Key.Right) _particleCount = (int)(_particleCount * up);
                    if (e.Key == Key.Left) _particleCount = (int)(_particleCount * down);
                    reinitialize = true;
                    break;
            }
            if (reinitialize) InitializeParticles(_particleCount);
        }
    }
}