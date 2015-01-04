using System;
using ObjectTK;
using ObjectTK.Shaders;
using ObjectTK.Tools;
using ObjectTK.Tools.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Examples
{
    /// <summary>
    /// Provides common functionality for the examples.
    /// </summary>
    public class ExampleWindow
        : DerpWindow
    {
        protected Camera Camera;
        protected Matrix4 ModelView;
        protected Matrix4 Projection;
        protected string OriginalTitle { get; private set; }

        public ExampleWindow()
            : base(800, 600, GraphicsMode.Default, "")
        {
            // disable vsync
            VSync = VSyncMode.Off;
            // set up camera
            Camera = new Camera();
            Camera.SetBehavior(new ThirdPersonBehavior());
            Camera.DefaultState.Position.Z = 5;
            Camera.ResetToDefault();
            Camera.Enable(this);
            ResetMatrices();
            // hook up events
            Load += OnLoad;
            Unload += OnUnload;
            KeyDown += OnKeyDown;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // maximize window
            WindowState = WindowState.Maximized;
            // remember original title
            OriginalTitle = Title;
            // set search path for shader files and extension
            ProgramFactory.BasePath = "Data/Shaders/";
            ProgramFactory.Extension = "glsl";
        }

        private void OnUnload(object sender, EventArgs e)
        {
            // release all gl resources on unload
            GLResource.DisposeAll(this);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // display FPS in the window title
            Title = string.Format("ObjectTK example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            // close window on escape press
            if (e.Key == Key.Escape) Close();
            // reset camera to default position and orientation on R press
            if (e.Key == Key.R) Camera.ResetToDefault();
        }

        /// <summary>
        /// Resets the ModelView and Projection matrices to the identity.
        /// </summary>
        protected void ResetMatrices()
        {
            ModelView = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        /// <summary>
        /// Sets a perspective projection matrix and applies the camera transformation on the modelview matrix.
        /// </summary>
        protected void SetupPerspective()
        {
            // setup perspective projection
            var aspectRatio = Width / (float)Height;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000);
            ModelView = Matrix4.Identity;
            // apply camera transform
            ModelView = Camera.GetCameraTransform();
        }
    }
}