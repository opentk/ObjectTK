using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using SphFluid.Core;
using SphFluid.Properties;
using SphFluid.Simulation.Shaders;

namespace SphFluid
{
    public abstract class GameBase
        : GameWindow
    {
        protected readonly QFont Font;
        protected readonly Camera Camera;
        protected readonly SimpleShader SimpleShader;

        protected FrameTimer FrameTimer;
        protected Matrix4 ModelView;
        protected Matrix4 Projection;

        protected GameBase(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            VSync = VSyncMode.Off;
            Font = new QFont(Path.Combine(Settings.Default.FontDir, "Comfortaa-Regular.ttf"), 16);
            Camera = new Camera(this);
            SimpleShader = new SimpleShader();
            FrameTimer = new FrameTimer();
            ResetMatrices();
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
