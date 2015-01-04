using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Examples.BasicExamples
{
    [ExampleProject("Custom buffer memory layout and simple texturing")]
    public class BufferLayoutTexturingExample
        : ExampleWindow
    {
        /// <summary>
        /// Defines a custom struct which is uploaded to a buffer object.
        /// The StructLayout attributes makes sure that the order of data in memory is like it is defined here.
        /// To prevent errors with byte-offsets read up on "packing".
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector3 Position;
            public Vector2 TexCoord;

            public Vertex(float x, float y, float z, float u, float v)
            {
                Position = new Vector3(x,y,z);
                TexCoord = new Vector2(u,v);
            }
        }
        
        private Texture2D _texture;
        private Sampler _sampler;
        private SimpleTextureProgram _program;
        private Buffer<Vertex> _vbo;
        private VertexArray _vao;
        private bool _enableMipmapping;

        public BufferLayoutTexturingExample()
        {
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
            KeyDown += OnKeyDown;
            _enableMipmapping = true;
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space: _enableMipmapping = !_enableMipmapping; break;
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // load texture from file
            using (var bitmap = new Bitmap("Data/Textures/checker.jpg"))
            {
                BitmapTexture.CreateCompatible(bitmap, out _texture);
                _texture.LoadBitmap(bitmap);
            }
            _texture.GenerateMipMaps();

            // initialize sampler
            _sampler = new Sampler();
            _sampler.SetWrapMode(TextureWrapMode.Repeat);

            // create vertex data for a big plane
            const int a = 10;
            const int b = 10;
            var vertices = new[]
            {
                new Vertex(-a, 0,-a, 0, 0),
                new Vertex( a, 0,-a, b, 0),
                new Vertex(-a, 0, a, 0, b),
                new Vertex( a, 0, a, b, b)
            };
            
            // create buffer object and upload vertex data
            _vbo = new Buffer<Vertex>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertices);

            // initialize shader
            _program = ProgramFactory.Create<SimpleTextureProgram>();
            // activate shader program
            _program.Use();
            // bind sampler
            _sampler.Bind(TextureUnit.Texture0);
            // bind texture
            _program.Texture.BindTexture(TextureUnit.Texture0, _texture);
            // which is equivalent to
            //_program.Texture.Set(TextureUnit.Texture0);
            //_texture.Bind(TextureUnit.Texture0);

            // set up vertex array and attributes
            _vao = new VertexArray();
            _vao.Bind();
            // memory layout of our data is XYZUVXYZUV...
            // the buffer abstraction knows the total size of one "pack" of vertex data
            // and if a vertex attribute is bound without further arguments the first N elements are taken from each pack
            // where N is provided via the VertexAttribAttribute on the program property:
            _vao.BindAttribute(_program.InPosition, _vbo);
            // if data should not be taken from the start of each pack, the offset must be given in bytes
            // to reach the texture coordinates UV the XYZ coordinates must be skipped, that is 3 floats, i.e. an offset of 12 bytes is needed
            _vao.BindAttribute(_program.InTexCoord, _vbo, 12);
            // if needed all the available arguments can be specified manually, e.g.
            //_vao.BindAttribute(_program.InTexCoord, _vbo, 2, VertexAttribPointerType.Float, Marshal.SizeOf(typeof(Vertex)), 12, false);

            // set default camera
            Camera.DefaultState.Position = new Vector3(0, 0.5f, 3);
            Camera.ResetToDefault();

            // set a nice clear color
            GL.ClearColor(Color.MidnightBlue);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // enable/disable mipmapping on the sampler
            _sampler.SetParameter(SamplerParameterName.TextureMinFilter,
                (int)(_enableMipmapping ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Nearest));
            
            // set transformation matrix
            _program.ModelViewProjectionMatrix.Set(ModelView*Projection);
            // render vertex data
            _vao.DrawArrays(PrimitiveType.TriangleStrip, 0, _vbo.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}