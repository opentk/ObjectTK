using System;
using ObjectTK;
using ObjectTK.Shaders;
using ObjectTK.Tools;
using ObjectTK.Tools.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;

namespace Examples
{
    /// <summary>
    /// Provides common functionality for the examples.
    /// </summary>
    public class ExampleWindow
        : CameraWindow
    {
        protected Matrix4 ModelView;
        protected Matrix4 Projection;
        protected string OriginalTitle { get; private set; }

        public ExampleWindow()
            : base(800, 600, "Meow")
        {
            ResetMatrices();
        }

        protected override void OnLoad()
        {
            // maximize window
            WindowState = WindowState.Maximized;
            // remember original title
            OriginalTitle = Title;
            // set search path for shader files and extension
            ProgramFactory.BasePath = "Data/Shaders/";
            ProgramFactory.Extension = "glsl";
            MakeCurrent();
        }

        protected override void OnUnload()
        {
            // release all gl resources on unload
            GLResource.DisposeAll(this);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // display FPS in the window title
            Title = string.Format("ObjectTK example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            // close window on escape press
            if (e.Key == Key.Escape) Close();
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
            var aspectRatio = Size.X / (float)Size.Y;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000);
            ModelView = Matrix4.Identity;
            // apply camera transform
            ModelView = ActiveCamera.GetCameraTransform();
        }
    }
}