using ObjectTK.Tools;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Examples {
	public class ExampleWindow : CameraWindow {
		private string OriginalTitle { get; set; }

		private float CameraPitch { get; set; } = 0;
		private float CameraYaw { get; set; } = (float)Math.PI;

		public ExampleWindow() : base(800, 600, "ExampleWindow") {

		}

		protected override void OnLoad() {
			base.OnLoad();
			// maximize window
			WindowState = WindowState.Maximized;
			// remember original title
			OriginalTitle = Title;

			ActiveCamera.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, CameraYaw) * Quaternion.FromAxisAngle(Vector3.UnitX, CameraPitch);

		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			base.OnRenderFrame(e);
			// display FPS in the window title
			Title = string.Format("ObjectTK example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
		}

		protected override void OnUpdateFrame(FrameEventArgs e) {
			base.OnUpdateFrame(e);
			HandleCameraInput((float)e.Time);
		}

		private void HandleCameraInput(float DeltaSeconds) {
			Vector3 Direction = Vector3.Zero;
			if (KeyboardState.IsKeyDown(Keys.W)) Direction += ActiveCamera.Forward;
			if (KeyboardState.IsKeyDown(Keys.S)) Direction -= ActiveCamera.Forward;
			if (KeyboardState.IsKeyDown(Keys.D)) Direction += ActiveCamera.Right;
			if (KeyboardState.IsKeyDown(Keys.A)) Direction -= ActiveCamera.Right;
			if (KeyboardState.IsKeyDown(Keys.Space)) Direction += ActiveCamera.Up;
			if (KeyboardState.IsKeyDown(Keys.LeftControl)) Direction -= ActiveCamera.Up;
			if (Direction.LengthSquared > 0) ActiveCamera.Position += Direction.Normalized() * DeltaSeconds;
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e) {
			base.OnKeyDown(e);
			if (e.Key == Keys.Escape) Close();
		}


		protected override void OnMouseMove(MouseMoveEventArgs e) {
			base.OnMouseMove(e);
			if (MouseState.IsButtonDown(MouseButton.Button2)) {
				CameraPitch -= e.DeltaY * 0.005f;
				CameraPitch = Math.Clamp(CameraPitch, -MathHelper.PiOver2, MathHelper.PiOver2);
				CameraYaw += e.DeltaX * 0.005f;
				ActiveCamera.Rotation = Quaternion.FromAxisAngle(-Vector3.UnitY, CameraYaw) * Quaternion.FromAxisAngle(-Vector3.UnitX, CameraPitch);
			}
		}

	}
}