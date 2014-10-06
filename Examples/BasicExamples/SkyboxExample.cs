using System;
using System.Drawing;
using DerpGL.Shapes;
using DerpGL.Textures;
using Examples.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.BasicExamples
{
    [ExampleProject("Skybox rendering with cube map texture")]
    public class SkyboxExample
        : ExampleWindow
    {
        private SkyboxShader _shader;
        private TextureCubemap _skybox;
        private Cube _cube;

        public SkyboxExample()
            : base("Skybox rendering with cube map texture")
        {
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRender;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize cube shape
            _cube = new Cube();
            _cube.UpdateBuffers();
            // initialize shader
            _shader = new SkyboxShader();
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
            // enable seamless filtering to reduce artifacts at the edges of the cube faces
            GL.Enable(EnableCap.TextureCubeMapSeamless);
            // cull front faces because we are inside the cube
            // this is not really necessary but removes some artifacts when the user leaves the cube
            // which should be impossible for a real skybox, but in this demonstration it is possible by zooming out
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
        }

        private void OnUnload(object sender, EventArgs e)
        {
            _cube.VertexBuffer.Dispose();
            _cube.IndexBuffer.Dispose();
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // render skybox
            _shader.Use();
            _shader.InPosition.Bind(_cube.VertexBuffer);
            // note: normally you want to clear the translation part of the ModelView matrix to prevent the user from leaving the cube
            // to do that you can use ModelView.ClearTranslation() instead of the unmodified ModelView matrix
            _shader.ModelViewProjectionMatrix.Set(Matrix4.CreateScale(20) * ModelView * Projection);
            _shader.Texture.BindTexture(TextureUnit.Texture0, _skybox);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _cube.IndexBuffer.Handle);
            GL.DrawElements(_cube.DefaultMode, _cube.IndexBuffer.ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // swap buffers
            SwapBuffers();
        }
    }
}
