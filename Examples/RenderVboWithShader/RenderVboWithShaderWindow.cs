using System;
using System.Drawing;
using DerpGL;
using DerpGL.Buffers;
using DerpGL.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Examples.RenderVboWithShader
{
    public class RenderVboWithShaderWindow
        : DerpWindow
    {
        private ExampleShader _shader;
        private Buffer<Vector3> _vbo;

        public RenderVboWithShaderWindow(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // set shader search path
            Shader.BasePath = "Data/RenderVboWithShader/Shaders/";
            
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
            _shader.InVertex.Bind(_vbo);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vbo.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}