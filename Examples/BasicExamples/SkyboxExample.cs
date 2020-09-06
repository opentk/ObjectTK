using System;
using System.Drawing;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using ObjectTK.Tools.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Examples.BasicExamples
{
    [ExampleProject("Skybox rendering with cube map texture")]
    public class SkyboxExample
        : ExampleWindow
    {
        private SkyboxProgram _program;
        private TextureCubemap _skybox;
        private VertexArray _vao;
        private Cube _cube;

		protected override void OnLoad() {
            base.OnLoad();
            // initialize shader
            _program = ProgramFactory.Create<SkyboxProgram>();
            // initialize cube shape
            _cube = new Cube();
            _cube.UpdateBuffers();
            // initialize vertex array and attributes
            _vao = new VertexArray();
            _vao.Bind();
            _vao.BindAttribute(_program.InPosition, _cube.VertexBuffer);
            _vao.BindElementBuffer(_cube.IndexBuffer);
            // create cubemap texture and load all faces
            for (var i = 0; i < 6; i++)
            {
                using (var bitmap = new Bitmap(string.Format("Data/Textures/city{0}.jpg", i)))
                {
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (_skybox == null) BitmapTexture.CreateCompatible(bitmap, out _skybox, 1);
                    _skybox.LoadBitmap(bitmap, i);
                }
            }
            // activate shader and bind texture to it
            _program.Use();
            _program.Texture.BindTexture(TextureUnit.Texture0, _skybox);
            // enable seamless filtering to reduce artifacts at the edges of the cube faces
            GL.Enable(EnableCap.TextureCubeMapSeamless);
            // cull front faces because we are inside the cube
            // this is not really necessary but removes some artifacts when the user leaves the cube
            // which should be impossible for a real skybox, but in this demonstration it is possible by zooming out
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            // set a nice clear color
            GL.ClearColor(Color.MidnightBlue);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _cube.VertexBuffer.Dispose();
            _cube.IndexBuffer.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // set up viewport
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // note: normally you want to clear the translation part of the ModelView matrix to prevent the user from leaving the cube
            // to do that you can use ModelView.ClearTranslation() instead of the unmodified ModelView matrix
            _program.ModelViewProjectionMatrix.Set(Matrix4.CreateScale(10) * ActiveCamera.ViewProjectionMatrix);
            // draw cube
            _vao.DrawElements(_cube.DefaultMode, _cube.IndexBuffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}
