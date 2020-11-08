using ObjectTK.Data.Variables;
using ObjectTK.Extensions.Shaders;
using ObjectTK.Extensions.Variables;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Examples.Examples.Programs {

	[VertexShaderSource("BasicShader.Vertex")]
	[FragmentShaderSource("BasicShader.Fragment")]
	public class BasicProgram {
		[VertexAttrib(VertexAttribPointerType.Float)]
		public ShaderAttributeInfo InPosition { get; set; }
		public ShaderUniformInfo<Matrix4> ModelViewProjectionMatrix { get; set; }
	}
}
