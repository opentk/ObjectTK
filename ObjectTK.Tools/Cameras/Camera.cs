//
// Camera.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace ObjectTK.Tools.Cameras {
	public class Camera {
		public Vector3 Position { get; set; }
		public Vector3 Forward { get; private set; } = -Vector3.UnitZ;
		public Vector3 Up { get; private set; } = Vector3.UnitY;
		public Vector3 Right => Vector3.Cross(Forward, Up);
		
		public Matrix4 GetCameraTransform() {
			// kind of hack: prevent look-at and up directions to be parallel
			if (Math.Abs(Vector3.Dot(Up, Forward)) > 0.99999999999) Forward += 0.001f * new Vector3(3, 5, 4);
			return Matrix4.LookAt(Position, Position + Forward, Up);
		}

		public void Rotate(Vector3 EulerAngles) {
			Forward = Vector3.Transform(Forward, Quaternion.FromEulerAngles(EulerAngles)).Normalized();
		}

		public void LookAt(Vector3 Point) {
			Forward = (Point - Position).Normalized();
		}

	}
}

// for later use..
//public Vector3 GetPickingRay(int mouseX, int mouseY)
//{
//    const float fieldOfView = MathHelper.PiOver4;
//    const float nearClippingPaneDistance = 1;
//    var view = -Position;
//    view.Normalize();
//    var cameraUp = Vector3.UnitY;
//    var h = Vector3.Cross(view, cameraUp);
//    h.Normalize();
//    var v = Vector3.Cross(h, view);
//    v.Normalize();
//    var vLength = (float)Math.Tan(fieldOfView / 2) * nearClippingPaneDistance;
//    var hLength = vLength * ((float)_window.Width / _window.Height);
//    v = v * vLength;
//    h = h * hLength;
//    //Vector3.Multiply(ref v, vLength, out v);
//    //Vector3.Multiply(ref h, hLength, out h);
//    // scale mouse position to [-1,1]
//    var x = 2f * mouseX / _window.Width - 1;
//    var y = 1 - 2f * mouseY / _window.Height;
//    var pos = Position + view * nearClippingPaneDistance + h * x + v * y;
//    var dir = pos - Position;
//    return dir;
//}