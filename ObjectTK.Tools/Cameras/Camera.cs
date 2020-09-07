//
// Camera.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using ObjectTK.Tools.Mathematics;
using OpenTK.Mathematics;

namespace ObjectTK.Tools.Cameras {
	public class Camera {
		public Vector3 Position { get; set; } = Vector3.Zero;
		public Quaternion Rotation { get; set; } = Quaternion.Identity;
		public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, Rotation).Normalized();
		public Vector3 Up => Vector3.UnitY;
		public Vector3 Right => Vector3.Cross(Forward, Up);
		public float FieldOfView { get; set; } = MathHelper.PiOver3;
		public float AspectRatio { get; set; } = 1.0f;
		public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + Forward, Up);
		public Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearClippingPlaneDistance, FarClippingPlaneDistance);
		public Matrix4 ViewProjectionMatrix => ViewMatrix * ProjectionMatrix;

		public float NearClippingPlaneDistance => 0.1f;
		public float FarClippingPlaneDistance => 1000.0f;

		public Matrix4 GetCameraTransform() {
			// kind of hack: prevent look-at and up directions to be parallel
			//if (Math.Abs(Vector3.Dot(Up, Forward)) > 0.99999999999) Forward += 0.001f * new Vector3(3, 5, 4);
			return Matrix4.LookAt(Position, Position + Forward, Up);
		}

		public void Rotate(Vector3 EulerAngles) {
			Quaternion Delta = (Matrix4.CreateFromAxisAngle(Right, EulerAngles.X) * Matrix4.CreateFromAxisAngle(Vector3.UnitY, EulerAngles.Y)).ExtractRotation();
			Rotation = Delta * Rotation;
		}

		public void LookAt(Vector3 Point) {
			Rotation = Matrix4.LookAt(Position, Point, Up).ExtractRotation();
		}

		public Ray GetPickingRay(Vector2 normalizedMousePos) {

			Vector4 screenPos = new Vector4(normalizedMousePos.X, normalizedMousePos.Y, -NearClippingPlaneDistance, 1.0f);

			Matrix4 Unview = Matrix4.Invert(Matrix4.Transpose(ViewMatrix));
			Matrix4 Unproject = Matrix4.Invert(Matrix4.Transpose(ProjectionMatrix));

			Vector4 Unprojected = Unproject * screenPos;
			Unprojected.Z = -1;
			Unprojected.W = 0;
			Vector3 direction = (Unview * Unprojected).Xyz;
			direction.Normalize();
			
			return new Ray { Origin = Position, Direction = direction, Length = 1.0f };
		}

	}
}