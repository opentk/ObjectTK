using System;
using System.Drawing;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using ObjectTK.Tools.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Examples.AdvancedExamples
{
    [ExampleProject("Render to texture")]
    public class RenderToTextureExample
        : ExampleWindow
    {
        private const int FramebufferWidth = 400;
        private const int FramebufferHeight = 400;
        
        private Framebuffer _framebuffer;
        private Renderbuffer _depthBuffer;
        private Texture2D _texture;

        private SimpleColorProgram _colorProgram;
        private SimpleTextureProgram _textureProgram;
        
        private Shape _cube;
        private Shape _quad;


        protected override void OnLoad()
        {
            base.OnLoad();

            // initialize and bind framebuffer
            _framebuffer = new Framebuffer();
            _framebuffer.Bind(FramebufferTarget.Framebuffer);

            // initialize a renderbuffer and bind it to the depth attachment
            // to support depth testing while rendering to the texture
            _depthBuffer = new Renderbuffer();
            _depthBuffer.Init(RenderbufferStorage.DepthComponent, FramebufferWidth, FramebufferHeight);
            _framebuffer.Attach(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, _depthBuffer);

            // initialize texture and bind it to the color attachment
            _texture = new Texture2D(SizedInternalFormat.Rgba8, FramebufferWidth, FramebufferHeight, 1);
            _framebuffer.Attach(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _texture);
            Framebuffer.Unbind(FramebufferTarget.Framebuffer);

            // initialize shaders
            _colorProgram = ProgramFactory.Create<SimpleColorProgram>();
            _textureProgram = ProgramFactory.Create<SimpleTextureProgram>();

            // initialize demonstration geometry
            _cube = ShapeBuilder.CreateColoredCube(_colorProgram.InPosition, _colorProgram.InColor);
            _quad = ShapeBuilder.CreateTexturedQuad(_textureProgram.InPosition, _textureProgram.InTexCoord);

            // set camera position
            ActiveCamera.Position = new Vector3(0,0,3);

            // enable depth testing
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // set up render to texture
            _framebuffer.Bind(FramebufferTarget.Framebuffer);
            GL.Viewport(0, 0, FramebufferWidth, FramebufferHeight);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // render rotating cube to texture
            _colorProgram.Use();
            _colorProgram.ModelViewProjectionMatrix.Set(
                Matrix4.CreateRotationX((float) FrameTimer.TimeRunning/1000)
                * Matrix4.CreateRotationY((float) FrameTimer.TimeRunning/1000)
                * Matrix4.CreateTranslation(0,0,-5)
                * Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, FramebufferWidth/(float)FramebufferHeight, 0.1f, 100));

            _cube.Draw();

            // reset to default framebuffer
            Framebuffer.Unbind(FramebufferTarget.Framebuffer);
            
            // set up viewport for the window
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            // render quad with texture
            _textureProgram.Use();
            _textureProgram.ModelViewProjectionMatrix.Set(ActiveCamera.ViewProjectionMatrix);
            
            _quad.Draw();

            // swap buffers
            SwapBuffers();
        }
    }
}