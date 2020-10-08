using System;
using System.Drawing;
using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Tools.Shapes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace Examples.BasicExamples {

	public class MeshData {
		public List<float> Vertices { get; set; }
		public List<uint> Indices { get; set; }
	}

	[ExampleProject("Example on how to use the new shape API for loading a mesh")]
	public class MeshLoadExample
		: ExampleWindow {
		private SimpleColorProgram _program;

		DynamicShape mesh;

		float TotalTime = 0;

		protected override void OnLoad() {
			base.OnLoad();
			// initialize shader (load sources, create/compile/link shader program, error checking)
			// when using the factory method the shader sources are retrieved from the ShaderSourceAttributes
			_program = ProgramFactory.Create<SimpleColorProgram>();
			// this program will be used all the time so just activate it once and for all
			_program.Use();


			//arbitrary black box to load mesh data
			string jsonString = File.ReadAllText("./Data/Meshes/OpenTK.fakeformat");
			MeshData meshData = JsonSerializer.Deserialize<MeshData>(jsonString);


			Buffer<uint> IndexBuffer = new Buffer<uint>();
			IndexBuffer.Init(BufferTarget.ElementArrayBuffer, meshData.Indices.Select(index => index - 1).ToArray());

			Buffer<Vector3> VertBuffer = new Buffer<Vector3>();
			VertBuffer.Init(BufferTarget.ArrayBuffer, Enumerable.Range(0, meshData.Vertices.Count / 3).Select(a => new Vector3(meshData.Vertices[a * 3], meshData.Vertices[a * 3 + 1], meshData.Vertices[a * 3 + 2])).ToArray());

			//a bit of a hack, i wanted the mesh to have some visual depth. 
			//the only reason this works is I just happen to know the Z coordinate for the mesh is in a certain range
			//other meshes will either look stupid or just throw exceptions because the color values are out of range
			Buffer<uint> ColorBuffer = new Buffer<uint>();
			ColorBuffer.Init(BufferTarget.ArrayBuffer, VertBuffer.Content.Select(vertex => (uint)Color.FromArgb((int)(vertex.Z * 500) + 100, (int)(vertex.Z * 500) + 100, (int)(vertex.Z * 500) + 100).ToArgb()).ToArray());
			

			mesh = new DynamicShape()
				.WithVertexAttrib(_program.InPosition, VertBuffer)
				.WithVertexAttrib(_program.InColor, ColorBuffer)
				.WithElementBuffer(IndexBuffer)
				.WithDisposeFunction(() => {
					VertBuffer?.Dispose();
					IndexBuffer?.Dispose();
					ColorBuffer?.Dispose();
				})
				.WithDrawFunction((VAO) => {
					VAO.DrawElements(PrimitiveType.Triangles, IndexBuffer.ElementCount);
				});

			// set camera position
			ActiveCamera.Position = new Vector3(0, 0, 3);

			// set a nice clear color
			GL.ClearColor(Color.MidnightBlue);

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
		}

		private void OnUnload(object sender, EventArgs e) {
			base.OnUnload();
			// Always make sure to properly dispose gl resources to prevent memory leaks.
			// Most of the examples do not explicitly dispose resources, because
			// the base class (ExampleWindow) calls GLResource.DisposeAll(this).
			// This will automatically dispose all objects referenced by class fields
			// which derive from GLResource. Everything else still has to be disposed manually.
			_program.Dispose();

			mesh.Dispose();

		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			base.OnRenderFrame(e);
			// set up viewport
			GL.Viewport(0, 0, Size.X, Size.Y);
			// clear the back buffer
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			TotalTime += (float)e.Time;
			Matrix4 Model = Matrix4.CreateRotationY(TotalTime);


			// calculate the MVP matrix and set it to the shaders uniform
			_program.ModelViewProjectionMatrix.Set(Model * ActiveCamera.ViewProjectionMatrix);

			mesh.Draw();

			// swap buffers
			SwapBuffers();
		}
	}
}