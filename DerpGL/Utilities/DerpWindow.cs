#region License
// DerpGL License
// Copyright (C) 2013-2014 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using System;
using log4net;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Utilities
{
    /// <summary>
    /// Provides basic functionality to an OpenTK GameWindow such as camera controls,
    /// ModelView and Projection matrices and improved timing.
    /// </summary>
    public abstract class DerpWindow
        : GameWindow
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DerpWindow));

        protected readonly FrameTimer FrameTimer;

        /// <summary>
        /// Initializes a new instance of the DerpWindow class.
        /// </summary>
        protected DerpWindow(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            // log some OpenGL information
            Logger.Info("OpenGL context information:");
            Logger.InfoFormat("{0}: {1}", StringName.Vendor, GL.GetString(StringName.Vendor));
            Logger.InfoFormat("{0}: {1}", StringName.Renderer, GL.GetString(StringName.Renderer));
            Logger.InfoFormat("{0}: {1}", StringName.Version, GL.GetString(StringName.Version));
            Logger.InfoFormat("{0}: {1}", StringName.ShadingLanguageVersion, GL.GetString(StringName.ShadingLanguageVersion));
            int numExtensions;
            GL.GetInteger(GetPName.NumExtensions, out numExtensions);
            Logger.DebugFormat("Number available extensions: {0}", numExtensions);
            for (var i = 0; i < numExtensions; i++) Logger.DebugFormat("{0}: {1}", i, GL.GetString(StringNameIndexed.Extensions, i));
            Logger.InfoFormat("Initializing game window: {0}", title);
            // set up GameWindow events
            Resize += OnResize;
            UpdateFrame += OnUpdateFrame;
            // set up frame timer
            FrameTimer = new FrameTimer();
        }

        private void OnResize(object sender, EventArgs eventArgs)
        {
            Logger.InfoFormat("Window resized to: {0}x{1}", Width, Height);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            FrameTimer.Time();
        }
    }
}
