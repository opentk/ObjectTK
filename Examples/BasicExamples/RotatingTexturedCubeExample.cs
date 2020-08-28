using System;
using System.Diagnostics;
using System.Drawing;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using ObjectTK.Tools.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Examples.BasicExamples
{
    [ExampleProject("Rotating textured cube rendering")]
    public class RotatingTexturedCubeExample
        : ExampleWindow
    {
        private Texture2D _texture;

        private SimpleTextureProgram _textureProgram;

        private TexturedCube _cube;
        private VertexArray _cubeVao;

        private Matrix4 _baseView;
        private Matrix4 _objectView;

        private Vector3[] _rotateVectors = new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.One };
        private const int _defaultRotateIndex = 4;

        private int _rotateIndex = _defaultRotateIndex;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public RotatingTexturedCubeExample()
        {
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    _objectView = _baseView = Matrix4.Identity;
                    _rotateIndex = _defaultRotateIndex;
                    _stopwatch.Restart();
                    break;

                case Key.Space:
                    _baseView = _objectView;
                    _rotateIndex = (_rotateIndex + 1) % _rotateVectors.Length;
                    _stopwatch.Restart();
                    break;

                case Key.Number0:
                case Key.Number1:
                case Key.Number2:
                case Key.Number3:
                case Key.Number4:
                    _baseView = _objectView;
                    _rotateIndex = (e.Key - Key.Number0) % _rotateVectors.Length;
                    _stopwatch.Restart();
                    break;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
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
            _cube = new TexturedCube();
            _cube.UpdateBuffers();

            // set up vertex attributes for the quad
            _cubeVao = new VertexArray();
            _cubeVao.Bind();
            _cubeVao.BindAttribute(_textureProgram.InPosition, _cube.VertexBuffer);
            _cubeVao.BindAttribute(_textureProgram.InTexCoord, _cube.TexCoordBuffer);

            // Enable culling, our cube vertices are defined inside out, so we flip them
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            // initialize camera position
            Camera.DefaultState.Position = new Vector3(0, 0, 4);
            Camera.ResetToDefault();

            // set nice clear color
            GL.ClearColor(Color.MidnightBlue);

            _stopwatch.Restart();
        }

        private void OnRenderFrame(object sender, OpenTK.FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // determinate object view rotation vectors and apply them
            _objectView = _baseView;
            var rotation = _rotateVectors[_rotateIndex];
            if (rotation != Vector3.Zero)
                _objectView *= Matrix4.CreateFromAxisAngle(_rotateVectors[_rotateIndex], (float)(_stopwatch.Elapsed.TotalSeconds * 1.0));

            // set transformation matrix
            _textureProgram.Use();
            _textureProgram.ModelViewProjectionMatrix.Set(_objectView * ModelView * Projection);

            // render cube with texture
            _cubeVao.Bind();
            _cubeVao.DrawArrays(_cube.DefaultMode, 0, _cube.VertexBuffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}
