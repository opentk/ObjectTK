using System;
using DerpGL;
using DerpGL.Shaders;
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
        public ExampleWindow(string title)
            : base(800, 600, GraphicsMode.Default, string.Format("DerpGL example: {0}", title))
        {
            Load += OnLoad;
            Unload += OnUnload;
            KeyDown += OnKeyDown;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // set search path for shader files
            Shader.BasePath = "Data/Shaders/";
        }

        private void OnUnload(object sender, EventArgs e)
        {
            // release all gl resources on unload
            GLResource.DisposeAll(this);
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            // close window on escape press
            if (e.Key == Key.Escape) Close();
        }
    }
}