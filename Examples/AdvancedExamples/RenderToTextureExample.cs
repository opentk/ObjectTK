using System;
using System.Drawing;
using DerpGL.Buffers;
using DerpGL.Shapes;
using DerpGL.Textures;
using Examples.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.AdvancedExamples
{
    [ExampleProject("Render to 2D texture via framebuffer")]
    public class RenderToTextureExample
        : ExampleWindow
    {
        private const int FramebufferWidth = 400;
        private const int FramebufferHeight = 400;
        
        private FrameBuffer _framebuffer;
        private RenderBuffer _depthBuffer;
        private Texture2D _texture;

        private SimpleColorShader _colorShader;
        private SimpleTextureShader _textureShader;
        
        private ColorCube _cube;
        private TexturedQuad _quad;

        public RenderToTextureExample()
            : base("Render to texture")
        {
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize and bind framebuffer
            _framebuffer = new FrameBuffer();
            _framebuffer.Bind();

            // initialize a renderbuffer and bind it to the depth attachment
            // to support depth testing while rendering to the texture
            _depthBuffer = new RenderBuffer();
            _depthBuffer.Init(RenderbufferStorage.DepthComponent, FramebufferWidth, FramebufferHeight);
            _framebuffer.Attach(FramebufferAttachment.DepthAttachment, _depthBuffer);

            // initialize texture and bind it to the color attachment
            _texture = new Texture2D(SizedInternalFormat.Rgba8, FramebufferWidth, FramebufferHeight, 1);
            _framebuffer.Attach(FramebufferAttachment.ColorAttachment0, _texture);
            _framebuffer.Unbind();

            // initialize shaders
            _colorShader = new SimpleColorShader();
            _textureShader = new SimpleTextureShader();

            // initialize demonstration geometry
            _cube = new ColorCube();
            _cube.UpdateBuffers();
            _quad = new TexturedQuad();
            _quad.UpdateBuffers();

            // enable depth testing
            GL.Enable(EnableCap.DepthTest);
        }

        private void OnUnload(object sender, EventArgs e)
        {
            _cube.VertexBuffer.Dispose();
            _cube.ColorBuffer.Dispose();
            _cube.IndexBuffer.Dispose();
            _quad.VertexBuffer.Dispose();
            _quad.TexCoordBuffer.Dispose();
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up render to texture
            _framebuffer.Bind();
            GL.Viewport(0, 0, FramebufferWidth, FramebufferHeight);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // render rotating cube to texture
            _colorShader.Use();
            _colorShader.ModelViewProjectionMatrix.Set(
                Matrix4.CreateRotationX((float) FrameTimer.TimeRunning/1000)
                * Matrix4.CreateRotationY((float) FrameTimer.TimeRunning/1000)
                * Matrix4.CreateTranslation(0,0,-5)
                * Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, FramebufferWidth/(float)FramebufferHeight, 0.1f, 100));
            _colorShader.InPosition.Bind(_cube.VertexBuffer);
            _colorShader.InColor.Bind(_cube.ColorBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _cube.IndexBuffer.Handle);
            GL.DrawElements(PrimitiveType.Triangles, _cube.IndexBuffer.ElementCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // reset to default framebuffer
            _framebuffer.Unbind();
            
            // set up viewport for the window
            GL.Viewport(0, 0, Width, Height);
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();
            
            // render quad with texture
            _textureShader.Use();
            _textureShader.ModelViewProjectionMatrix.Set(ModelView * Projection);
            _textureShader.InPosition.Bind(_quad.VertexBuffer);
            _textureShader.InTexCoord.Bind(_quad.TexCoordBuffer);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, _quad.VertexBuffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}