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
using System.Collections.Generic;
using log4net;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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

        protected readonly Camera Camera;
        protected readonly FrameTimer FrameTimer;
        protected Matrix4 ModelView;
        protected Matrix4 Projection;

        protected readonly List<MouseButton> MouseButtons;

        /// <summary>
        /// Initializes a new instance of the DerpWindow class.
        /// </summary>
        protected DerpWindow(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title)
        {
            // get open gl information
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
            VSync = VSyncMode.Off;
            // set up mouse events
            MouseButtons = new List<MouseButton>();
            Mouse.ButtonDown += OnMouseDown;
            Mouse.ButtonUp += OnMouseUp;
            // set up camera
            Camera = new Camera(this);
            ResetMatrices();
            // set up GameWindow events
            Load += OnLoad;
            Resize += OnResize;
            UpdateFrame += OnUpdateFrame;
            // set up frame timer
            FrameTimer = new FrameTimer();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            WindowState = WindowState.Maximized;
        }

        private void OnResize(object sender, EventArgs eventArgs)
        {
            Logger.InfoFormat("Window resized to: {0}x{1}", Width, Height);
        }

        private void OnUpdateFrame(object sender, FrameEventArgs e)
        {
            FrameTimer.Time();
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
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 100);
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
