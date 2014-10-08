using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DerpGL;
using DerpGL.Buffers;
using DerpGL.Textures;
using Examples.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.BasicExamples
{
    [ExampleProject("Textured grid rendering")]
    public class TextureGridExample
        : ExampleWindow
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Minefield
        {
            public Vector2 Position;
            public int State;

            public Minefield(float x, float y, int texture)
            {
                Position = new Vector2(x, y);
                State = texture;
            }
        }

        private TextureGridShader _gridShader;
        private Texture2DArray _textureArray;
        private Buffer<Minefield> _buffer;

        private readonly string[] _stateTextures = { "empty.png", "flag.png", "mine.png" };

        const int FieldWidth = 1000;
        const int FieldHeight = 1000;

        public TextureGridExample()
            : base("Textured grid rendering")
        {
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        protected void OnLoad(object sender, EventArgs e)
        {
            // load shader
            _gridShader = new TextureGridShader();
            // load textures into array
            for (var i = 0; i < _stateTextures.Length; i++)
            {
                using (var bitmap = new Bitmap(Path.Combine("Data/Textures/", _stateTextures[i])))
                {
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    if (_textureArray == null) BitmapTexture.CreateCompatible(bitmap, out _textureArray, _stateTextures.Length, 1);
                    _textureArray.LoadBitmap(bitmap, i);
                }
            }
            // initialize buffer
            var field = new Minefield[FieldWidth*FieldHeight];
            for (var i = 0; i < field.Length; i++)
            {
                field[i] = new Minefield(i%FieldWidth, i/FieldHeight, i%_stateTextures.Length);
            }
            _buffer = new Buffer<Minefield>();
            _buffer.Init(BufferTarget.ArrayBuffer, field);
            // initialize camera position
            Camera.Pitch = 0;
            Camera.Yaw = 0;
            Camera.Position = new Vector3(0, 0, 15);
        }

        private void OnUnload(object sender, EventArgs e)
        {
            GLResource.DisposeAll(this);
        }

        protected void OnRenderFrame(object sender, FrameEventArgs frameEventArgs)
        {
            // display FPS in the window title
            Title = string.Format("{0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);

            // setup stuff
            GL.Viewport(0, 0, Width, Height);
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            SetupPerspective();

            // render grid
            _gridShader.Use();
            _gridShader.InPosition.Bind(_buffer);
            _gridShader.InTexture.Bind(_buffer, 8);
            _gridShader.TextureData.BindTexture(TextureUnit.Texture0, _textureArray);
            _gridShader.ModelViewProjectionMatrix.Set(Matrix4.CreateTranslation(-500,-500,0) * ModelView * Projection);
            GL.DrawArrays(PrimitiveType.Points, 0, _buffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}