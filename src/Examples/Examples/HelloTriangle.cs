using System.Drawing;
using ObjectTK;
using ObjectTK._2D;
using ObjectTK.GLObjects;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Examples.Examples {
    internal static class ShaderSource {
        public const string Vertex = @"
				#version 330 core

				uniform mat4 ModelViewProjectionMatrix;

				layout(location = 0) in vec3 InPosition;
				layout(location = 1) in vec4 InColor;


				out vec4 VColor;

				void main(void) {
					gl_Position = ModelViewProjectionMatrix * vec4(InPosition, 1.0);
					VColor = InColor;
				}
			";

        public const string Fragment = @"
			#version 330

			in vec4 VColor;

			out vec4 FragColor;

			void main()
			{
				FragColor = VColor;
			}
		";
    }
    
    [ExampleProject("Hello Triangle")]
    public sealed class HelloTriangle : GameWindow {

	    private static readonly NativeWindowSettings WindowSettings = new NativeWindowSettings {
		    Size = new Vector2i(800, 600),
		    Title = "Hello Triangle (Basic)",
	    };
	    private static readonly GameWindowSettings GameWindowSettings = new GameWindowSettings();
	    
        private ShaderProgram _shaderProgram;
        private VertexArray _vao;
        private Buffer<Vector3> _positionsVbo;
        private Buffer<Color4> _colorsVbo;
        private readonly Camera2D _camera = new Camera2D();

        public HelloTriangle()
            : base(GameWindowSettings, WindowSettings) {
        }

        protected override void OnLoad() {
            base.OnLoad();
            
            // create the shader program
            _shaderProgram = GLFactory.Shader.VertexFrag("Vertex Color", ShaderSource.Vertex, ShaderSource.Fragment);
            
            // create the triangle to draw
            var positions = new[] {new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0)};
            var colors = new[] {Color4.Cornsilk, Color4.OrangeRed, Color4.DarkOliveGreen};
            
            _positionsVbo = GLFactory.Buffer.ArrayBuffer("Positions", positions);
            _colorsVbo = GLFactory.Buffer.ArrayBuffer("Colors", colors);
            _vao = GLFactory.VertexArray.FromBuffers("Triangle", _positionsVbo, _colorsVbo);
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            // set up the viewport and camera (if the screen size has changed).
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = (float) Size.X / Size.Y;
            
            // clear the screen
            GL.ClearColor(Color.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            
            // set up the shader and the uniforms
            GL.UseProgram(_shaderProgram.Handle);
            var mvpMatrix = _camera.ViewProjection;
            GL.UniformMatrix4(_shaderProgram.Uniforms["ModelViewProjectionMatrix"].Location, false, ref mvpMatrix);

            // draw the triangle
            GL.BindVertexArray(_vao.Handle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vao.ElementCount);
            GL.BindVertexArray(0);
            
            // swap to display on the screen
            SwapBuffers();
        }

        protected override void OnUnload() {
            base.OnUnload();

            GL.DeleteProgram(_shaderProgram.Handle);
            GL.DeleteVertexArray(_vao.Handle);
            GL.DeleteBuffer(_positionsVbo.Handle);
        }
    }
}
	
