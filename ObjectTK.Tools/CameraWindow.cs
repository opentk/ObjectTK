//
// CameraWindow.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using ObjectTK.Logging;
using ObjectTK.Tools.Cameras;
using OpenTK.Windowing.Common.Input;

namespace ObjectTK.Tools
{
    /// <summary>
    /// Provides basic functionality to an OpenTK GameWindow such as camera controls,
    /// ModelView and Projection matrices and improved timing.
    /// </summary>
    public abstract class CameraWindow
        : GameWindow
    {
        private static readonly IObjectTKLogger Logger = LogFactory.GetLogger(typeof(CameraWindow));

        protected readonly FrameTimer FrameTimer;
        protected Camera ActiveCamera { get; set; } = new Camera();

        /// <summary>
        /// Initializes a new instance of the CameraWindow class.
        /// </summary>
        protected CameraWindow(int width, int height, string title)
            : base(new GameWindowSettings { }, new NativeWindowSettings { Size = new Vector2i(width, height), Title = title })
        {
            Logger?.InfoFormat("Initializing game window: {0}", title);
            // set up GameWindow events
            FrameTimer = new FrameTimer();
        }

        private void LogGLInfo() {
            // log some OpenGL information
            Logger?.Info("OpenGL context information:");
            Logger?.InfoFormat("{0}: {1}", StringName.Vendor, GL.GetString(StringName.Vendor));
            Logger?.InfoFormat("{0}: {1}", StringName.Renderer, GL.GetString(StringName.Renderer));
            Logger?.InfoFormat("{0}: {1}", StringName.Version, GL.GetString(StringName.Version));
            Logger?.InfoFormat("{0}: {1}", StringName.ShadingLanguageVersion, GL.GetString(StringName.ShadingLanguageVersion));
            int numExtensions;
            GL.GetInteger(GetPName.NumExtensions, out numExtensions);
            Logger?.DebugFormat("Number available extensions: {0}", numExtensions);
            for (var i = 0; i < numExtensions; i++) Logger?.DebugFormat("{0}: {1}", i, GL.GetString(StringNameIndexed.Extensions, i));
        }

        protected override void OnResize(ResizeEventArgs resizeEventArgs)
        {
            base.OnResize(resizeEventArgs);
            Logger?.InfoFormat("Window resized to: {0}x{1}", Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            base.OnUpdateFrame(frameEventArgs);
            FrameTimer.Time();

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

		protected override void OnMouseMove(MouseMoveEventArgs e) {
			base.OnMouseMove(e);

            ActiveCamera.Rotate(new Vector3(e.DeltaY, e.DeltaX, 0) * 0.05f);

		}

		protected override void OnLoad() {
            MakeCurrent();
            base.OnLoad();
            LogGLInfo();
        }
	}
}
