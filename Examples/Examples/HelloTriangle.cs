﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using Examples.Examples.Programs;
using ObjectTK.Data.Buffers;
using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using ObjectTK.Extensions.Buffers;
using ObjectTK.Extensions.Shaders;
using ObjectTK.Extensions.Variables;
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

			VertexShader VertexShader = new VertexShader(GL.CreateShader(ShaderType.VertexShader), null);
			FragmentShader FragmentShader = new FragmentShader(GL.CreateShader(ShaderType.FragmentShader), null);

			GL.ShaderSource(VertexShader.Handle, VertSource);
			GL.ShaderSource(FragmentShader.Handle, FragSource);
			GL.CompileShader(VertexShader.Handle);
			GL.CompileShader(FragmentShader.Handle);

			int ProgramHandle = GL.CreateProgram();

			GL.AttachShader(ProgramHandle, VertexShader.Handle);
			GL.AttachShader(ProgramHandle, FragmentShader.Handle);

			GL.LinkProgram(ProgramHandle);

			int UniformLocation = GL.GetUniformLocation(ProgramHandle, "ModelViewProjectionMatrix");
			GL.GetActiveUniform(ProgramHandle, UniformLocation, out int UniformSize, out ActiveUniformType UniformType);
			UniformInfo UI_InPosition = new UniformInfo(ProgramHandle, "ModelViewProjectionMatrix", UniformLocation, UniformSize, UniformType, UniformLocation > -1);

			ShaderProgram = new ShaderProgram(ProgramHandle, VertexShader, FragmentShader, new Dictionary<string, UniformInfo> { { "ModelViewProjectionMatrix", UI_InPosition } }, new Dictionary<string, VertexAttributeInfo> { });

			GL.DetachShader(ProgramHandle, VertexShader.Handle);
			GL.DetachShader(ProgramHandle, FragmentShader.Handle);
			GL.DeleteShader(VertexShader.Handle);
			GL.DeleteShader(FragmentShader.Handle);

			GL.UseProgram(ProgramHandle);

			var Vertices = new[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0) };

			VBO = new Buffer<Vector3>(GL.GenBuffer());
			VBO.ElementCount = 3;
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO.Handle);
			GL.BufferData(BufferTarget.ArrayBuffer, VBO.ElementSize * VBO.ElementCount, Vertices, BufferUsageHint.StaticDraw);

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
	
	[ExampleProject("Hello Triangle with extensions")]
	public class HelloTriangleWithExtensions : ExampleWindow {
		
		private ShaderProgram<BasicProgram> ShaderProgram;
		private Buffer<Vector3> VBO;
		private VertexArrayObject VAO;

		protected override void OnLoad() {
			base.OnLoad();

			ShaderProgram = new ProgramFactory() { BaseDirectory = "./Data/Shaders/" }.CreateProgram<BasicProgram>();
			ShaderProgram.Use();

			var Vertices = new[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0) };

			VBO = new Buffer<Vector3>(GL.GenBuffer());
			VBO.BufferData(BufferTarget.ArrayBuffer, Vertices);

			VAO = new VertexArrayObject(GL.GenVertexArray());
			VAO.BindVertexAttribute(ShaderProgram.Variables.InPosition, VBO);

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

			for (int X = 0; X < 20; X++) {
				for (int Y = 0; Y < 20; Y++) {
					for (int Z = 0; Z < 20; Z++) {

						Matrix4 MVP = Matrix4.CreateTranslation(new Vector3(X * 2, Y * 2, Z * 2)) * ActiveCamera.ViewProjectionMatrix;
						ShaderProgram.Variables.ModelViewProjectionMatrix.Set(MVP);
						GL.DrawArrays(PrimitiveType.Triangles, 0, VBO.ElementCount);

					}
				}
			}

			SwapBuffers();
		}
	}
}
