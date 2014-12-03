using System;
using DerpGL;
using DerpGL.Shaders;
using DerpGL.Utilities;
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
        protected string OriginalTitle { get; private set; }

        public ExampleWindow()
            : base(800, 600, GraphicsMode.Default, "")
        {
            Load += OnLoad;
            Unload += OnUnload;
            KeyDown += OnKeyDown;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
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
            Title = string.Format("DerpGL example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            // close window on escape press
            if (e.Key == Key.Escape) Close();
        }
    }
}