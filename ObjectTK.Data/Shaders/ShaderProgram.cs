using ObjectTK.Data.Variables;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ObjectTK.Data.Shaders {

	public class ShaderProgram {
		public int Handle { get; }
		public VertexShaderStage VertexShaderStage { get; }
		public FragmentShaderStage FragmentShaderStage { get; }
		public Dictionary<string, UniformInfo> Uniforms { get; set; }
		public Dictionary<string, VertexAttributeInfo> VertexAttributes { get; set; }

		public ShaderProgram(int Handle, VertexShaderStage vertexShaderStage, FragmentShaderStage fragmentShaderStage, Dictionary<string, UniformInfo> Uniforms, Dictionary<string, VertexAttributeInfo> VertexAttributes) {
			this.Handle = Handle;
			this.VertexShaderStage = vertexShaderStage;
			this.FragmentShaderStage = fragmentShaderStage;
			this.Uniforms = Uniforms;
			this.VertexAttributes = VertexAttributes;
		}
	}
}
