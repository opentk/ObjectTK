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
using ObjectTK.Tools.Mathematics;
using System.Collections.Generic;
using ObjectTK.Buffers;

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
            ActiveCamera.AspectRatio = Size.X / (float) Size.Y;
            Logger?.InfoFormat("Window resized to: {0}x{1}", Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            base.OnUpdateFrame(frameEventArgs);
            FrameTimer.Time();
        }

		protected override void OnLoad() {
            MakeCurrent();
            base.OnLoad();
            LogGLInfo();
        }
	}
}
