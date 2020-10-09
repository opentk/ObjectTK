using ObjectTK.Buffers;
using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ObjectTK.Tools.Shapes {
	public static class ShapeBuilder {

		private static DynamicShape CreateBasicCube(VertexAttrib VertexPositionAttrib) {

			Vector3[] Vertices = new[]
			{
				new Vector3(-1.0f, -1.0f,  1.0f),
				new Vector3( 1.0f, -1.0f,  1.0f),
				new Vector3( 1.0f,  1.0f,  1.0f),
				new Vector3(-1.0f,  1.0f,  1.0f),
				new Vector3(-1.0f, -1.0f, -1.0f),
				new Vector3( 1.0f, -1.0f, -1.0f),
				new Vector3( 1.0f,  1.0f, -1.0f),
				new Vector3(-1.0f,  1.0f, -1.0f)
			};

			uint[] Indices = new uint[]
			{
                // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                5, 1, 0, 0, 4, 5,
                // right face
                1, 5, 6, 6, 2, 1
			};

			Buffer<Vector3> VertexBuffer = new Buffer<Vector3>();
			VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
			Buffer<uint> IndexBuffer = new Buffer<uint>();
			IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices);

			return new DynamicShape()
				.WithElementBuffer(IndexBuffer)
				.WithVertexAttrib(VertexPositionAttrib, VertexBuffer)
				.WithDisposeFunction(() => {
					VertexBuffer?.Dispose();
					IndexBuffer?.Dispose();
				})
				.SetDrawFunction((VertexArray VAO) => {
					VAO.DrawElements(PrimitiveType.Triangles, IndexBuffer.ElementCount);
				});

		}

		public static Shape CreateCube(VertexAttrib VertexPositionAttrib) {
			return CreateBasicCube(VertexPositionAttrib);
		}

		public static Shape CreateColoredCube(VertexAttrib VertexPositionAttrib, VertexAttrib VertexColorAttrib) {

			uint[] Colors = new List<Color>
{
				Color.DarkRed,
				Color.DarkRed,
				Color.Gold,
				Color.Gold,
				Color.DarkRed,
				Color.DarkRed,
				Color.Gold,
				Color.Gold
			}.Select(_ => _.ToRgba32()).ToArray();

			Buffer<uint> ColorBuffer = new Buffer<uint>();
			ColorBuffer.Init(BufferTarget.ArrayBuffer, Colors);

			return CreateBasicCube(VertexPositionAttrib)
				.WithVertexAttrib(VertexColorAttrib, ColorBuffer)
				.WithDisposeFunction(() => {
					ColorBuffer?.Dispose();
				});
		}

		public static Shape CreateTexturedCube(VertexAttrib VertexPositionAttrib, VertexAttrib VertexTexCoordAttrib) {

			var quad_uv_map = new[]
			{
				new Vector2(0, 0),
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(1, 1),
				new Vector2(1, 0),
				new Vector2(0, 0),
			};


			Vector3[] Vertices = new[]
			{
				new Vector3(-1.0f, -1.0f,  1.0f),
				new Vector3( 1.0f, -1.0f,  1.0f),
				new Vector3( 1.0f,  1.0f,  1.0f),
				new Vector3(-1.0f,  1.0f,  1.0f),
				new Vector3(-1.0f, -1.0f, -1.0f),
				new Vector3( 1.0f, -1.0f, -1.0f),
				new Vector3( 1.0f,  1.0f, -1.0f),
				new Vector3(-1.0f,  1.0f, -1.0f)
			};

			uint[] Indices = new uint[]
			{
                // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                5, 1, 0, 0, 4, 5,
                // right face
                1, 5, 6, 6, 2, 1
			};

			Vertices = Indices.Select(idx => Vertices[idx]).ToArray();

			// Use predefined uv texture mapping for vertices
			Vector2[] TexCoords = Enumerable.Range(0, Vertices.Length).Select(i => quad_uv_map[i % quad_uv_map.Length]).ToArray();

			Buffer<Vector3> VertexBuffer = new Buffer<Vector3>();
			VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
			Buffer<Vector2> TexCoordBuffer = new Buffer<Vector2>();
			TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);

			return new DynamicShape()
				.WithVertexAttrib(VertexPositionAttrib, VertexBuffer)
				.WithVertexAttrib(VertexTexCoordAttrib, TexCoordBuffer)
				.WithDisposeFunction(() => {
					VertexBuffer?.Dispose();
					TexCoordBuffer?.Dispose();
				})
				.SetDrawFunction((VertexArray VAO) => {
					VAO.DrawArrays(PrimitiveType.Triangles, 0, VertexBuffer.ElementCount);
				});

		}

		private static DynamicShape CreateBasicQuad(VertexAttrib VertexPositionAttrib) {

			Vector3[] Vertices = new[]
			{
				new Vector3(-1, -1, 0),
				new Vector3( 1, -1, 0),
				new Vector3(-1,  1, 0),
				new Vector3( 1,  1, 0)
			};

			Buffer<Vector3> VertexBuffer = new Buffer<Vector3>();
			VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);

			return new DynamicShape()
				.WithVertexAttrib(VertexPositionAttrib, VertexBuffer)
				.WithDisposeFunction(() => {
					VertexBuffer?.Dispose();
				})
				.SetDrawFunction((VertexArray VAO) => {
					VAO.DrawArrays(PrimitiveType.TriangleStrip, 0, VertexBuffer.ElementCount);
				});
		}

		public static Shape CreateQuad(VertexAttrib VertexPositionAttrib) {
			return CreateBasicQuad(VertexPositionAttrib);
		}

		public static Shape CreateTexturedQuad(VertexAttrib VertexPositionAttrib, VertexAttrib VertexTexCoordAttrib) {

			Vector2[] TexCoords = new[]
{
				new Vector2(0,0),
				new Vector2(1,0),
				new Vector2(0,1),
				new Vector2(1,1)
			};

			Buffer<Vector2> TexCoordBuffer = new Buffer<Vector2>();
			TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);

			return CreateBasicQuad(VertexPositionAttrib)
				.WithVertexAttrib(VertexTexCoordAttrib, TexCoordBuffer)
				.WithDisposeFunction(() => {
					TexCoordBuffer?.Dispose();
				});

		}

		public static Shape CreateCircle(VertexAttrib VertexPositionAttrib, float radius = 1, int slices = 64) {

			float dtheta = MathHelper.TwoPi / (slices - 1);
			var theta = 0f;
			Vector3[] Vertices = new Vector3[slices + 1];
			Vertices[0] = new Vector3(0, 0, 0);
			for (var i = 0; i < slices; i++) {
				Vertices[i + 1] = new Vector3((float)Math.Cos(theta) * radius, (float)Math.Sin(theta) * radius, 0);
				theta += dtheta;
			}

			Buffer<Vector3> VertexBuffer = new Buffer<Vector3>();
			VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);

			return new DynamicShape()
				.WithVertexAttrib(VertexPositionAttrib, VertexBuffer)
				.WithDisposeFunction(() => {
					VertexBuffer?.Dispose();
				})
				.SetDrawFunction((VertexArray VAO) => {
					VAO.DrawArrays(PrimitiveType.TriangleFan, 0, VertexBuffer.ElementCount);
				});
		}

	}

}
