using System;
using System.Drawing;
using DerpGL.Buffers;
using Examples.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.BasicExamples
{
    [ExampleProject("Minimal example on shader and buffer usage")]
    public class MinimalExample
        : ExampleWindow
    {
        private ExampleShader _shader;
        private Buffer<Vector3> _vbo;

        public MinimalExample()
            : base("Shader and buffer usage")
        {
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize shader (load sources, create/compile/link shader program, error checking)
            _shader = new ExampleShader();
            
            // create some vertices
            var vertices = new[] { new Vector3(-1,-1,0), new Vector3(1,-1,0), new Vector3(0,1,0) };
            
            // create buffer object and upload vertex data
            _vbo = new Buffer<Vector3>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertices);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // render vertices
            _shader.Use();
            _shader.ModelViewProjectionMatrix.Set(ModelView*Projection);
            _shader.InPosition.Bind(_vbo);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vbo.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}