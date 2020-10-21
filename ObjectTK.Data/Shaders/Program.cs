using ObjectTK.Data.Variables;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ObjectTK.Data.Shaders {

	public class Program {
		public int Handle { get; }
		public VertexShader VertexShader { get; }
		public FragmentShader FragmentShader { get; }
		public Dictionary<string, UniformInfo> Uniforms { get; set; }
		public Dictionary<string, VertexAttributeInfo> VertexAttributes { get; set; }

		public Program(int Handle, VertexShader VertexShader, FragmentShader FragmentShader, Dictionary<string, UniformInfo> Uniforms, Dictionary<string, VertexAttributeInfo> VertexAttributes) {
			this.Handle = Handle;
			this.VertexShader = VertexShader;
			this.FragmentShader = FragmentShader;
			this.Uniforms = Uniforms;
			this.VertexAttributes = VertexAttributes;
		}
	}
}
