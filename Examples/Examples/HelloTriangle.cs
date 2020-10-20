
using System;
using System.Drawing;
using ObjectTK.Data.Buffers;
using ObjectTK.Data.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Examples.Examples {
	
	[ExampleProject("Hello Triangle")]
	public class HelloTriangle : ExampleWindow {

		private Program ShaderProgram;
		private int VBOHandle;
		private VertexArray VAO;
		
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

			ShaderProgram = new Program(GL.CreateProgram(), VertexShader, FragmentShader);

			GL.AttachShader(ShaderProgram.Handle, VertexShader.Handle);
			GL.AttachShader(ShaderProgram.Handle, FragmentShader.Handle);

			GL.LinkProgram(ShaderProgram.Handle);

			GL.DetachShader(ShaderProgram.Handle, VertexShader.Handle);
			GL.DetachShader(ShaderProgram.Handle, FragmentShader.Handle);
			GL.DeleteShader(VertexShader.Handle);
			GL.DeleteShader(FragmentShader.Handle);

			GL.UseProgram(ShaderProgram.Handle);

			GL.GetProgram(ShaderProgram.Handle, GetProgramParameterName.LinkStatus, out var code);
			if (code != (int)All.True) {
				throw new Exception($"Error occurred whilst linking Program({ShaderProgram.Handle})");
			}


			var Vertices = new[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0) };

			VBOHandle = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);
			GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float) * 3, Vertices, BufferUsageHint.StaticDraw);


			VAO = new VertexArray(GL.GenVertexArray());
			GL.BindVertexArray(VAO.Handle);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBOHandle);

			ActiveCamera.Position = new Vector3(0, 0, 3);

			GL.ClearColor(Color.MidnightBlue);
		}

		private void OnUnload(object sender, EventArgs e) {
			base.OnUnload();

			GL.DeleteProgram(ShaderProgram.Handle);
			GL.DeleteVertexArray(VAO.Handle);
			GL.DeleteBuffer(VBOHandle);
		}

		protected override void OnRenderFrame(FrameEventArgs e) {
			base.OnRenderFrame(e);
			GL.Viewport(0, 0, Size.X, Size.Y);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 MVPMatrix = ActiveCamera.ViewProjectionMatrix;
			GL.UniformMatrix4(0, false, ref MVPMatrix);

			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

			SwapBuffers();
		}
	}
}
