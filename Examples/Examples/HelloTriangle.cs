
using System;
using System.Collections.Generic;
using System.Drawing;
using ObjectTK.Data;
using ObjectTK.Data.Buffers;
using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Examples.Examples {

	[ExampleProject("Hello Triangle")]
	public sealed class HelloTriangle : ExampleWindow {

		private ShaderProgram _shaderProgram;
		private Buffer<Vector3> _vbo;
		private VertexArrayObject _vao;

		private const string VertSource = @"
				#version 330 core

				uniform mat4 ModelViewProjectionMatrix;
				layout(location = 0) in vec3 InPosition;

				void main(void) {
					gl_Position = ModelViewProjectionMatrix * vec4(InPosition, 1.0);
				}
			";

		private const string FragSource = @"
				#version 330	

				out vec4 FragColor;

				void main()
				{
					FragColor = vec4(1);
				}
			";

		protected override void OnLoad() {
			base.OnLoad();

			var shaderProgram = GLFactory.Shader.VertexFrag("Solid Color", VertSource, FragSource);
			_shaderProgram = shaderProgram;
			GL.UseProgram(shaderProgram.Handle);
			
			
			var vertices = new[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0) };
			_vbo = GLFactory.Buffer.ArrayBuffer("Positions", vertices);
			_vao = GLFactory.VAO.FromBuffers("Triangle", _vbo);
			
			ActiveCamera.Position = new Vector3(0, 0, 3);

			GL.ClearColor(Color.MidnightBlue);
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			base.OnRenderFrame(e);
			GL.Viewport(0, 0, Size.X, Size.Y);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var mvpMatrix = ActiveCamera.ViewProjectionMatrix;
			GL.UniformMatrix4(_shaderProgram.Uniforms["ModelViewProjectionMatrix"].Location, false, ref mvpMatrix);

			GL.BindVertexArray(_vao.Handle);
			GL.DrawArrays(PrimitiveType.Triangles, 0, _vao.ElementCount);
			GL.BindVertexArray(0);
			SwapBuffers();
		}
		
		protected override void OnUnload() {
			base.OnUnload();

			GL.DeleteProgram(_shaderProgram.Handle);
			GL.DeleteVertexArray(_vao.Handle);
			GL.DeleteBuffer(_vbo.Handle);
		}
	}
}
