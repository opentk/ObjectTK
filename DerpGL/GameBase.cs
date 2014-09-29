using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DerpGL.Properties;
using log4net;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using QuickFont;

namespace DerpGL
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

        protected readonly List<MouseButton> MouseButtons;

        protected GameBase(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            // get open gl information
            Logger.Info("OpenGL context information:");
            Logger.InfoFormat("{0}: {1}", StringName.Vendor, GL.GetString(StringName.Vendor));
            Logger.InfoFormat("{0}: {1}", StringName.Renderer, GL.GetString(StringName.Renderer));
            Logger.InfoFormat("{0}: {1}", StringName.Version, GL.GetString(StringName.Version));
            Logger.InfoFormat("{0}: {1}", StringName.ShadingLanguageVersion, GL.GetString(StringName.ShadingLanguageVersion));
            Logger.DebugFormat("{0}:\n{1}", StringName.Extensions, string.Join("",
                GL.GetString(StringName.Extensions).Split(' ').Select((_,i) => string.Format("{0}{1}", _, i%4==3?"\n":"\t"))));
            Logger.InfoFormat("Initializing game window: {0}", title);
            var fontPath = Path.Combine(Settings.Default.FontDir, "Comfortaa-Regular.ttf");
            Logger.InfoFormat("Loading font: {0}", fontPath);
            Font = new QFont(fontPath, 16);
            VSync = VSyncMode.Off;
            Camera = new Camera(this);
            FrameTimer = new FrameTimer();
            ResetMatrices();
            MouseButtons = new List<MouseButton>();
            Mouse.ButtonDown += OnMouseDown;
            Mouse.ButtonUp += OnMouseUp;
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
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 10);
            ModelView = Matrix4.Identity;
            // apply camera transform
            Camera.ApplyCamera(ref ModelView);
        }

        /// <summary>
        /// Unbinds the first n textures and resets the active framebuffer to default buffer, i.e. the screen.
        /// </summary>
        /// <param name="n">The number of textures to unbind.</param>
        public static void ResetState(int n)
        {
#if DEBUG
            for (var i = 0; i < n; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.BindTexture(TextureTarget.Texture2DArray, 0);
                GL.BindTexture(TextureTarget.TextureBuffer, 0);
                GL.DisableVertexAttribArray(i);
            }
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.UseProgram(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.TransformFeedbackBuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
#endif
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!MouseButtons.Contains(e.Button)) MouseButtons.Add(e.Button);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtons.Contains(e.Button)) MouseButtons.Remove(e.Button);
        }

        protected Vector3 CalculateMousePosition()
        {
            // intersect ray with Z = 0 plane
            var eye = Camera.Position;
            var ray = Camera.GetPickingRay(Mouse.X, Mouse.Y);
            var t = -eye.Z / ray.Z;
            var position = eye + t * ray;
            // apply camera rotation
            Camera.ApplyRotation(ref position);
            return position;
        }
    }
}
