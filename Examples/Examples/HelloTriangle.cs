
using System;
using System.Collections.Generic;
using System.Drawing;
using ObjectTK.Data.Buffers;
using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Examples.Examples {

	[ExampleProject("Hello Triangle")]
	public class HelloTriangle : ExampleWindow {

		private ShaderProgram ShaderProgram;
		private Buffer<Vector3> VBO;
		private VertexArrayObject VAO;

		private readonly string VertSource = @"
				#version 330 core

				uniform mat4 ModelViewProjectionMatrix;
				layout(location = 0) in vec3 InPosition;

				void main(void) {
					gl_Position = ModelViewProjectionMatrix * vec4(InPosition, 1.0);
				}
			";
		private readonly string FragSource = @"
				#version 330	

				out vec4 FragColor;

				void main()
				{
					FragColor = vec4(1);
				}
			";

		protected override void OnLoad() {
			base.OnLoad();

			var shaderProgram = ShaderCompiler.VertexFrag("Solid Color", VertSource, FragSource);
			GL.UseProgram(shaderProgram.Handle);
			ShaderProgram = shaderProgram;

			var vertices = new[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0) };

			VBO = new Buffer<Vector3>(GL.GenBuffer(), 0);
			VBO.ElementCount = 3;
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO.Handle);
			GL.BufferData(BufferTarget.ArrayBuffer, VBO.ElementSize * VBO.ElementCount, vertices, BufferUsageHint.StaticDraw);

			VAO = new VertexArrayObject(GL.GenVertexArray());
			GL.BindVertexArray(VAO.Handle);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO.Handle);

			ActiveCamera.Position = new Vector3(0, 0, 3);

			GL.ClearColor(Color.MidnightBlue);
		}

		private void OnUnload(object sender, EventArgs e) {
			base.OnUnload();

			GL.DeleteProgram(ShaderProgram.Handle);
			GL.DeleteVertexArray(VAO.Handle);
			GL.DeleteBuffer(VBO.Handle);
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			base.OnRenderFrame(e);
			GL.Viewport(0, 0, Size.X, Size.Y);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 MVPMatrix = ActiveCamera.ViewProjectionMatrix;
			GL.UniformMatrix4(ShaderProgram.Uniforms["ModelViewProjectionMatrix"].Location, false, ref MVPMatrix);

			GL.DrawArrays(PrimitiveType.Triangles, 0, VBO.ElementCount);

			SwapBuffers();
		}
	}
}
