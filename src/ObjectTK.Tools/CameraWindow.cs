using System;

namespace ObjectTK.Tools {
	using ObjectTK.Tools.Cameras;
	using OpenTK.Mathematics;
	using OpenTK.Windowing.Common;
	using OpenTK.Windowing.Desktop;

	public abstract class CameraWindow : GameWindow {

		protected readonly FrameTimer FrameTimer;


		private Camera _ActiveCamera = new Camera();
		protected Camera ActiveCamera {
			get {
				return _ActiveCamera;
			}
			set {
				_ActiveCamera = value;
				_ActiveCamera.Viewport = new Box2i(Vector2i.Zero, Size);
			}
		}

		protected CameraWindow(int width, int height, string title)
			: base(new GameWindowSettings { }, new NativeWindowSettings { Size = new Vector2i(width, height), Title = title }) {
			FrameTimer = new FrameTimer();
		}

		protected override void OnResize(ResizeEventArgs resizeEventArgs) {
			base.OnResize(resizeEventArgs);
			_ActiveCamera.Viewport = new Box2i(Vector2i.Zero, Size);
		}

		protected override void OnUpdateFrame(FrameEventArgs frameEventArgs) {
			base.OnUpdateFrame(frameEventArgs);
			FrameTimer.Time();
		}

	}
}