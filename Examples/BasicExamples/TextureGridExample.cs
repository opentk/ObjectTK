using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DerpGL.Buffers;
using DerpGL.Shaders;
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

        private TextureGridProgram _gridProgram;
        private Texture2DArray _textureArray;
        private Buffer<Minefield> _buffer;
        private VertexArray _vao;

        private readonly string[] _stateTextures = { "empty.png", "flag.png", "mine.png" };

        const int FieldWidth = 1000;
        const int FieldHeight = 1000;

        public TextureGridExample()
            : base("Textured grid rendering")
        {
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
        }

        protected void OnLoad(object sender, EventArgs e)
        {
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
            // load program
            _gridProgram = ProgramFactory.Create<TextureGridProgram>();
            _gridProgram.Use();
            // bind the texture and set uniform
            _gridProgram.TextureData.BindTexture(TextureUnit.Texture0, _textureArray);
            // set up vertex array and attributes
            _vao = new VertexArray();
            _vao.Bind();
            _vao.BindAttribute(_gridProgram.InPosition, _buffer);
            _vao.BindAttribute(_gridProgram.InTexture, _buffer, 8);
            // set nice clear color
            GL.ClearColor(Color.MidnightBlue);
            // initialize camera position
            Camera.Pitch = 0;
            Camera.Yaw = 0;
            Camera.Position = new Vector3(0, 0, 15);
        }

        protected void OnRenderFrame(object sender, FrameEventArgs frameEventArgs)
        {
            // display FPS in the window title
            Title = string.Format("{0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);

            // setup stuff
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // update MVP matrix and render grid
            _gridProgram.ModelViewProjectionMatrix.Set(Matrix4.CreateTranslation(-500,-500,0) * ModelView * Projection);
            _vao.DrawArrays(PrimitiveType.Points, 0, _buffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}