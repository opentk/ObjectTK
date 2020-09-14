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
using System;

namespace ObjectTK.Tools.Cameras {

	public class Camera {
		public Vector3 Position { get; set; } = Vector3.Zero;
		public Quaternion Rotation { get; set; } = Quaternion.Identity;
		public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, Rotation).Normalized();
		public Vector3 Up => Vector3.Transform(Vector3.UnitY, Rotation).Normalized();
		public Vector3 Right => Vector3.Cross(Forward, Up);
		public float FieldOfView { get; set; } = MathHelper.PiOver2;
		public float AspectRatio => Viewport.Size.X / (float)Viewport.Size.Y;
		public Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + Forward, Up);

		public CameraProjectionType CameraProjectionType { get; set; } = CameraProjectionType.Perspective;
		public Matrix4 ProjectionMatrix => CameraProjectionType switch
		{
			CameraProjectionType.Perspective => Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearClippingPlaneDistance, FarClippingPlaneDistance),
			CameraProjectionType.Orthographic => Matrix4.CreateOrthographic(OrthographicVerticalSize * AspectRatio, OrthographicVerticalSize, NearClippingPlaneDistance, FarClippingPlaneDistance),
			_ => Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearClippingPlaneDistance, FarClippingPlaneDistance)
		};

		public Matrix4 ViewProjectionMatrix => ViewMatrix * ProjectionMatrix;
		public Box2i Viewport { get; set; }
		public float OrthographicVerticalSize { get; set; } = 5.0f;
		public float NearClippingPlaneDistance => 0.1f;
		public float FarClippingPlaneDistance => 1000.0f;

		public Ray GetPickingRay(Vector2 MousePosition) {

			Matrix4 UnViewProjectionMatrix = Matrix4.Invert(ViewProjectionMatrix);

			Vector3 Near = Vector3.Unproject(
				new Vector3(MousePosition.X, Viewport.Size.Y - MousePosition.Y, NearClippingPlaneDistance),
				Viewport.Min.X,
				Viewport.Min.Y,
				Viewport.Size.X,
				Viewport.Size.Y,
				NearClippingPlaneDistance,
				FarClippingPlaneDistance,
				UnViewProjectionMatrix);

			Vector3 Far = Vector3.Unproject(
				new Vector3(MousePosition.X, Viewport.Size.Y - MousePosition.Y, FarClippingPlaneDistance),
				Viewport.Min.X,
				Viewport.Min.Y,
				Viewport.Size.X,
				Viewport.Size.Y,
				NearClippingPlaneDistance,
				FarClippingPlaneDistance,
				UnViewProjectionMatrix);

			Vector3 Direction = (Far - Near).Normalized();

			return new Ray { Origin = Position, Direction = Direction, Length = 1.0f };
		}

	}
}