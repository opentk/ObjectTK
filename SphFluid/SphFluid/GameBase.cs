using System;
using System.IO;
using log4net;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using SphFluid.Core;
using SphFluid.Properties;

namespace SphFluid
{
    public abstract class GameBase
        : GameWindow
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GameBase));

        protected readonly QFont Font;
        protected readonly Camera Camera;

        protected FrameTimer FrameTimer;
        protected Matrix4 ModelView;
        protected Matrix4 Projection;

        protected GameBase(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            Logger.InfoFormat("Initializing game window: {0}", title);
            Font = new QFont(Path.Combine(Settings.Default.FontDir, "Comfortaa-Regular.ttf"), 16f);
            VSync = VSyncMode.Off;
            Camera = new Camera(this);
            FrameTimer = new FrameTimer();
            ResetMatrices();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowState = WindowState.Maximized;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Logger.InfoFormat("Window resized to: {0}x{1}", Width, Height);
        }

        protected void ResetMatrices()
        {
            ModelView = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        /// <summary>
        /// Sets the a perspective projection matrix and applies the camera transformation on the modelview matrix.
        /// </summary>
        protected void SetupPerspective()
        {
            // setup perspective projection
            var aspectRatio = Width / (float)Height;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 64);
            ModelView = Matrix4.Identity;
            // apply camera transform
            Camera.ApplyCamera(ref ModelView);
        }

        /// <summary>
        /// Unbinds texture 0 to n and resets the active framebuffer to default buffer, i.e. the screen.
        /// </summary>
        /// <param name="n">The number of textures to unbind.</param>
        public static void ResetState(int n)
        {
            for (var i = 0; i < n; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.TextureBuffer, 0);
                GL.DisableVertexAttribArray(i);
            }
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.UseProgram(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}
