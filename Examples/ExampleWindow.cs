﻿using System;
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
            base.OnLoad();
            // maximize window
            WindowState = WindowState.Maximized;
            // remember original title
            OriginalTitle = Title;
            // set search path for shader files and extension
            ProgramFactory.BasePath = "Data/Shaders/";
            ProgramFactory.Extension = "glsl";
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            // release all gl resources on unload
            GLResource.DisposeAll(this);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            // display FPS in the window title
            Title = string.Format("ObjectTK example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
        }

		protected override void OnUpdateFrame(FrameEventArgs frameEventArgs) {
			base.OnUpdateFrame(frameEventArgs);


            {
                var dir = Vector3.Zero;
                if (KeyboardState.IsKeyDown(Key.W)) dir += ActiveCamera.Forward;
                if (KeyboardState.IsKeyDown(Key.S)) dir -= ActiveCamera.Forward;
                if (KeyboardState.IsKeyDown(Key.D)) dir += ActiveCamera.Right;
                if (KeyboardState.IsKeyDown(Key.A)) dir -= ActiveCamera.Right;
                if (KeyboardState.IsKeyDown(Key.Space)) dir += ActiveCamera.Up;
                if (KeyboardState.IsKeyDown(Key.LControl)) dir -= ActiveCamera.Up;
                // normalize dir to enforce consistent movement speed, independent of the number of keys pressed
                if (dir.LengthSquared > 0) ActiveCamera.Position += dir.Normalized() * (float)frameEventArgs.Time;
            }
        }

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
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