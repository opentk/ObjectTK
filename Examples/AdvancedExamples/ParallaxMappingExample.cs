using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Examples.AdvancedExamples
{
    [ExampleProject("Swizzled Parallax Mapping (ported from OpenTK examples)")]
    public class ParallaxMappingExample
        : ExampleWindow
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector3 Tangent;
            public Vector2 TexCoord;
        }

        private VertexArray _vao;
        private Buffer<Vertex> _buffer;
        private ParallaxProgram _program;
        private Texture2D _textureDiffuseHeight;
        private Texture2D _textureNormalGloss;
        
        const string TextureDiffuseHeightFilename = "Data/Textures/swizzled-rock-diffuse-height.dds";
        const string TextureNormalGlossFilename = "Data/Textures/swizzled-rock-normal-gloss.dds";

        Vector3 _lightPosition = new Vector3(0.0f, 1.0f, 1.0f);
        Vector3 _lightDiffuse = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 _lightSpecular = new Vector3(1f, 1f, 1f);

        // Material parameter
        //private Vector3 _materialScaleAndBiasAndShininess = new Vector3( 0.07f, 0.0f, 38.0f ); // for Metal tex
        private Vector3 _materialScaleAndBiasAndShininess = new Vector3(0.04f, 0.0f, 92.0f); // for Rock tex

        public ParallaxMappingExample()
        {
            Load += OnLoad;
            Resize += OnResize;
            UpdateFrame += OnUpdateFrame;
            RenderFrame += OnRenderFrame;
        }

        protected void OnLoad(object sender, EventArgs eventArgs)
        {
            VSync = VSyncMode.Off;

            // Check for necessary capabilities:
            var extensions = GL.GetString(StringName.Extensions);
            if (!GL.GetString(StringName.Extensions).Contains("GL_ARB_shading_language"))
            {
                throw new NotSupportedException(String.Format("This example requires OpenGL 2.0. Found {0}. Aborting.",
                    GL.GetString(StringName.Version).Substring(0, 3)));
            }

            if (!extensions.Contains("GL_ARB_texture_compression") ||
                 !extensions.Contains("GL_EXT_texture_compression_s3tc"))
            {
                throw new NotSupportedException("This example requires support for texture compression. Aborting.");
            }

            var temp = new int[1];
            GL.GetInteger(GetPName.MaxTextureImageUnits, out temp[0]);
            Trace.WriteLine(temp[0] + " TMU's for Fragment Shaders found. (2 required)");

            GL.GetInteger(GetPName.MaxVaryingFloats, out temp[0]);
            Trace.WriteLine(temp[0] + " varying floats between VS and FS allowed. (6 required)");

            GL.GetInteger(GetPName.MaxVertexUniformComponents, out temp[0]);
            Trace.WriteLine(temp[0] + " uniform components allowed in Vertex Shader. (6 required)");

            GL.GetInteger(GetPName.MaxFragmentUniformComponents, out temp[0]);
            Trace.WriteLine(temp[0] + " uniform components allowed in Fragment Shader. (11 required)");
            Trace.WriteLine("");

            // load textures
            TextureLoaderParameters.MagnificationFilter = TextureMagFilter.Linear;
            TextureLoaderParameters.MinificationFilter = TextureMinFilter.LinearMipmapLinear;
            TextureLoaderParameters.WrapModeS = TextureWrapMode.ClampToBorder;
            TextureLoaderParameters.WrapModeT = TextureWrapMode.ClampToBorder;
            TextureLoaderParameters.EnvMode = TextureEnvMode.Modulate;

            uint handle;
            TextureTarget target;
            ImageDDS.LoadFromDisk(TextureDiffuseHeightFilename, out handle, out target);
            _textureDiffuseHeight = TextureFactory.AquireTexture2D((int) handle);
            Trace.WriteLine("Loaded " + TextureDiffuseHeightFilename + " with handle " + handle + " as " + target);

            ImageDDS.LoadFromDisk(TextureNormalGlossFilename, out handle, out target);
            _textureNormalGloss = TextureFactory.AquireTexture2D((int) handle);
            Trace.WriteLine("Loaded " + TextureNormalGlossFilename + " with handle " + handle + " as " + target);

            Trace.WriteLine("End of Texture Loading. GL Error: " + GL.GetError());
            Trace.WriteLine("");

            // initialize buffer
            var normal = Vector3.UnitZ;
            var tangent = Vector3.UnitX;
            var vertices = new[]
            {
                new Vertex
                {
                    Position = new Vector3(-1,-1,0),
                    TexCoord = new Vector2(0,0),
                    Normal = normal,
                    Tangent = tangent
                },
                new Vertex
                {
                    Position = new Vector3(1,-1,0),
                    TexCoord = new Vector2(1,0),
                    Normal = normal,
                    Tangent = tangent
                },
                new Vertex
                {
                    Position = new Vector3(-1,1,0),
                    TexCoord = new Vector2(0,1),
                    Normal = normal,
                    Tangent = tangent
                },
                new Vertex
                {
                    Position = new Vector3(1,1,0),
                    TexCoord = new Vector2(1,1),
                    Normal = normal,
                    Tangent = tangent
                }
            };
            _buffer = new Buffer<Vertex>();
            _buffer.Init(BufferTarget.ArrayBuffer, vertices);

            // load shader
            _program = ProgramFactory.Create<ParallaxProgram>();
            _program.Use();

            // set up vertex array
            _vao = new VertexArray();
            _vao.Bind();
            // bind vertex attributes
            // the buffer layout is defined by the Vertex struct:
            //   data X Y Z NX NY NZ TX TY TZ  U  V *next vertex*
            // offset 0 4 8 12 16 20 24 28 32 36 40      44
            // having to work with offsets could be prevented by using seperate buffer objects for each attribute,
            // but that might reduce performance and can get annoying as well.
            // performance increase could also be achieved by improving coherent read access
            // by padding the struct so that each attribute begins at a multiple of 16 bytes, i.e. 4-floats
            // because the GPU can then read all 4 floats at once. I did not do that here to keep it simple.
            _vao.BindAttribute(_program.InPosition, _buffer);
            _vao.BindAttribute(_program.InNormal, _buffer, 12);
            _vao.BindAttribute(_program.InTangent, _buffer, 24);
            _vao.BindAttribute(_program.InTexCoord, _buffer, 36);

            // set camera position
            Camera.DefaultState.Position = new Vector3(0,0,3);
            Camera.ResetToDefault();

            // set state
            GL.ClearColor(0.2f, 0f, 0.4f, 0f);
            GL.PointSize(10f);
            GL.Disable(EnableCap.Dither);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
        }

        protected void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            if (Keyboard[Key.Space]) Trace.WriteLine("GL: " + GL.GetError());
            var factor = (float)e.Time;
            if (Keyboard[Key.Q])
            {
                _materialScaleAndBiasAndShininess.X += factor;
                Trace.WriteLine("Scale: " + _materialScaleAndBiasAndShininess.X + " Bias: " + _materialScaleAndBiasAndShininess.Y);
            }
            if (Keyboard[Key.A])
            {
                _materialScaleAndBiasAndShininess.X -= factor;
                Trace.WriteLine("Scale: " + _materialScaleAndBiasAndShininess.X + " Bias: " + _materialScaleAndBiasAndShininess.Y);
            }
            if (Keyboard[Key.W])
            {
                _materialScaleAndBiasAndShininess.Y += factor;
                Trace.WriteLine("Scale: " + _materialScaleAndBiasAndShininess.X + " Bias: " + _materialScaleAndBiasAndShininess.Y);
            }
            if (Keyboard[Key.S])
            {
                _materialScaleAndBiasAndShininess.Y -= factor;
                Trace.WriteLine("Scale: " + _materialScaleAndBiasAndShininess.X + " Bias: " + _materialScaleAndBiasAndShininess.Y);
            }
            if (Keyboard[Key.E])
            {
                _materialScaleAndBiasAndShininess.Z += factor*100;
                Trace.WriteLine("Shininess: " + _materialScaleAndBiasAndShininess.Z);
            }
            if (Keyboard[Key.D])
            {
                _materialScaleAndBiasAndShininess.Z -= factor*100;
                Trace.WriteLine("Shininess: " + _materialScaleAndBiasAndShininess.Z);
            }

            _lightPosition.X = (-(Width / 2) + Mouse.X) / 100f;
            _lightPosition.Y = ((Height / 2) - Mouse.Y) / 100f;
        }

        protected void OnRenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            // first Material's uniforms
            _program.Material_DiffuseAndHeight.BindTexture(TextureUnit.Texture0, _textureDiffuseHeight);
            _program.Material_NormalAndGloss.BindTexture(TextureUnit.Texture1, _textureNormalGloss);
            _program.Material_ScaleBiasShininess.Set(_materialScaleAndBiasAndShininess);

            // the rest are vectors
            _program.Camera_Position.Set(Camera.State.Position);
            _program.Light_Position.Set(_lightPosition);
            _program.Light_DiffuseColor.Set(_lightDiffuse);
            _program.Light_SpecularColor.Set(_lightSpecular);

            // set up matrices
            SetupPerspective();
            var normalMatrix = new Matrix3(ModelView).Inverted();
            normalMatrix.Transpose();
            _program.NormalMatrix.Set(normalMatrix);
            _program.ModelViewMatrix.Set(ModelView);
            _program.ModelViewProjectionMatrix.Set(ModelView * Projection);

            // render
            _vao.DrawArrays(PrimitiveType.TriangleStrip, 0, _buffer.ElementCount);

            SwapBuffers();
        }
    }
}
