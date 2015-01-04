using System;
using System.Drawing;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.BasicExamples
{
    [ExampleProject("Minimal example on shader and buffer usage")]
    public class MinimalExample
        : ExampleWindow
    {
        private ExampleProgram _program;
        private VertexArray _vao;
        private Buffer<Vector3> _vbo;

        public MinimalExample()
        {
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize shader (load sources, create/compile/link shader program, error checking)
            // when using the factory method the shader sources are retrieved from the ShaderSourceAttributes
            _program = ProgramFactory.Create<ExampleProgram>();
            // this program will be used all the time so just activate it once and for all
            _program.Use();
            
            // create vertices for a triangle
            var vertices = new[] { new Vector3(-1,-1,0), new Vector3(1,-1,0), new Vector3(0,1,0) };
            
            // create buffer object and upload vertex data
            _vbo = new Buffer<Vector3>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertices);

            // create and bind a vertex array
            _vao = new VertexArray();
            _vao.Bind();
            // set up binding of the shader variable to the buffer object
            _vao.BindAttribute(_program.InPosition, _vbo);

            // set camera position
            Camera.DefaultState.Position = new Vector3(0,0,3);
            Camera.ResetToDefault();

            // set a nice clear color
            GL.ClearColor(Color.MidnightBlue);
        }

        private void OnUnload(object sender, EventArgs e)
        {
            // Always make sure to properly dispose gl resources to prevent memory leaks.
            // Most of the examples do not explicitly dispose resources, because
            // the base class (ExampleWindow) calls GLResource.DisposeAll(this).
            // This will automatically dispose all objects referenced by class fields
            // which derive from GLResource. Everything else still has to be disposed manually.
            _program.Dispose();
            _vao.Dispose();
            _vbo.Dispose();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            // clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // set up modelview and perspective matrix
            SetupPerspective();

            // calculate the MVP matrix and set it to the shaders uniform
            _program.ModelViewProjectionMatrix.Set(ModelView*Projection);
            // draw the buffer which contains the triangle
            _vao.DrawArrays(PrimitiveType.Triangles, 0, _vbo.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}