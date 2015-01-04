using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
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

        const int FieldWidth = 100;
        const int FieldHeight = 100;

        public TextureGridExample()
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
            Camera.DefaultState.Position = new Vector3(0, 5, 15);
            Camera.ResetToDefault();
        }

        protected void OnRenderFrame(object sender, FrameEventArgs frameEventArgs)
        {
            // setup stuff
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // update MVP matrix and render grid
            // also move camera rotation center to the center of the plane
            // and rotate the plane from the x-y plane to the x-z plane
            _gridProgram.ModelViewProjectionMatrix.Set(
                Matrix4.CreateTranslation(-FieldWidth/2, -FieldHeight/2, 0)
                * Matrix4.CreateRotationX(-(float)Math.PI/2)
                * ModelView * Projection);
            _vao.DrawArrays(PrimitiveType.Points, 0, _buffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}