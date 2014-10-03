using System;
using System.Drawing;
using System.Runtime.InteropServices;
using DerpGL.Buffers;
using DerpGL.Textures;
using Examples.Shaders;
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
        private SimpleTextureShader _shader;
        private Buffer<Vertex> _vbo;
        private bool _enableMipmapping;

        public BufferLayoutTexturingExample()
            : base("Custom buffer memory layout and texturing")
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
                _texture = new Texture2D(bitmap, 10);
            }
            _texture.GenerateMipMaps();

            // initialize sampler
            _sampler = new Sampler();
            _sampler.SetWrapMode(TextureWrapMode.Repeat);

            // initialize shader
            _shader = new SimpleTextureShader();
            
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

            // set default camera
            Camera.DefaultPosition.Y = 0.5f;
            Camera.ResetToDefault();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // enable/disable mipmapping
            _sampler.SetParameter(SamplerParameterName.TextureMinFilter,
                (int)(_enableMipmapping ? TextureMinFilter.NearestMipmapLinear : TextureMinFilter.Nearest));

            // activate shader program
            _shader.Use();
            // set transformation matrix
            _shader.ModelViewProjectionMatrix.Set(ModelView*Projection);
            // bind sampler
            _sampler.Bind(TextureUnit.Texture0);
            // bind texture
            _shader.Texture.BindTexture(TextureUnit.Texture0, _texture);
            // which is equivalent to
            //_shader.Texture.Set(TextureUnit.Texture0);
            //_texture.Bind(TextureUnit.Texture0);
            // memory layout of our data is XYZUVXYZUV...
            // the buffer abstraction knows the total size of one "pack" of vertex data
            // and if a vertex attribute is bound without further arguments the first N elements are taken from each pack
            // where N is provided via the VertexAttribAttribute on the shader property:
            _shader.InPosition.Bind(_vbo);
            // if data should not be taken from the start of each pack, the offset must be given in bytes
            // to reach the texture coordinates UV the XYZ coordinates must be skipped, that is 3 floats, i.e. an offset of 12 is needed
            _shader.InTexCoord.Bind(_vbo, 12);
            // if needed all the available arguments can be specified  manually, e.g.
            //_shader.InTexCoord.Bind(_vbo, 2, VertexAttribPointerType.Float, Marshal.SizeOf(typeof(Vertex)), 12, false);
            
            // render vertex data
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, _vbo.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}