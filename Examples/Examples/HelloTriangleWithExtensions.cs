using System;
using System.Drawing;
using Examples.Examples.Programs;
using ObjectTK.Data.Buffers;
using ObjectTK.Extensions.Buffers;
using ObjectTK.Extensions.Shaders;
using ObjectTK.Extensions.Variables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Examples.Examples {
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

            VBO = new Buffer<Vector3>(GL.GenBuffer(), 0);
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
