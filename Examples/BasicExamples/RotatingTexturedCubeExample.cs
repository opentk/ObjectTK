using System.Diagnostics;
using System.Drawing;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using ObjectTK.Tools.Shapes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Examples.BasicExamples
{
    [ExampleProject("Rotating textured cube rendering")]
    public class RotatingTexturedCubeExample
        : ExampleWindow
    {
        private Texture2D _texture;

        private SimpleTextureProgram _textureProgram;

        private Shape _cube;

        private Matrix4 _baseView;
        private Matrix4 _objectView;

        private Vector3[] _rotateVectors = new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.One };
        private const int _defaultRotateIndex = 4;

        private int _rotateIndex = _defaultRotateIndex;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Keys.R:
                    _objectView = _baseView = Matrix4.Identity;
                    _rotateIndex = _defaultRotateIndex;
                    _stopwatch.Restart();
                    break;

                case Keys.Space:
                    _baseView = _objectView;
                    _rotateIndex = (_rotateIndex + 1) % _rotateVectors.Length;
                    _stopwatch.Restart();
                    break;

                case Keys.KeyPad0:
                case Keys.KeyPad1:
                case Keys.KeyPad2:
                case Keys.KeyPad3:
                case Keys.KeyPad4:
                    _baseView = _objectView;
                    _rotateIndex = (e.Key - Keys.KeyPad0) % _rotateVectors.Length;
                    _stopwatch.Restart();
                    break;
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            // load texture from file
            using (var bitmap = new Bitmap("Data/Textures/crate.png"))
            {
                BitmapTexture.CreateCompatible(bitmap, out _texture);
                _texture.LoadBitmap(bitmap);
            }

            // initialize shaders
            _textureProgram = ProgramFactory.Create<SimpleTextureProgram>();

            // initialize cube object and base view matrix
            _objectView = _baseView = Matrix4.Identity;

            // initialize demonstration geometry
            _cube = ShapeBuilder.CreateTexturedCube(_textureProgram.InPosition, _textureProgram.InTexCoord);

            // Enable culling, our cube vertices are defined inside out, so we flip them
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            // initialize camera position
            ActiveCamera.Position = new Vector3(0, 0, 4);

            // set nice clear color
            GL.ClearColor(Color.MidnightBlue);

            _stopwatch.Restart();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // set up viewport
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // determinate object view rotation vectors and apply them
            _objectView = _baseView;
            var rotation = _rotateVectors[_rotateIndex];
            if (rotation != Vector3.Zero)
                _objectView *= Matrix4.CreateFromAxisAngle(_rotateVectors[_rotateIndex], (float)(_stopwatch.Elapsed.TotalSeconds * 1.0));

            // set transformation matrix
            _textureProgram.Use();
            _textureProgram.ModelViewProjectionMatrix.Set(_objectView * ActiveCamera.ViewProjectionMatrix);

            _cube.Draw();

            // swap buffers
            SwapBuffers();
        }
    }
}
