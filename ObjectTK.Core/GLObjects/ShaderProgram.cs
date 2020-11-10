using System.Collections.Generic;
using ObjectTK.Shaders;

namespace ObjectTK.GLObjects {

	/// See: https://www.khronos.org/opengl/wiki/Shader
	public class ShaderProgram {
		/// The OpenGL Handle
		public int Handle { get; }
		/// The individual stages making up this program in the order they are used.
		public ShaderStage[] Stages { get; set; }
		/// The uniforms on this shader.
		public Dictionary<string, ShaderUniformInfo> Uniforms { get; set; }
		/// The vertex attributes on this shader.
		public Dictionary<string, ShaderAttributeInfo> Attributes { get; set; }

		public ShaderProgram(int handle,
			ShaderStage[] stages,
			Dictionary<string, ShaderUniformInfo> uniforms,
			Dictionary<string, ShaderAttributeInfo> attributes) {
			Handle = handle;
			Stages = stages;
			Uniforms = uniforms;
			Attributes = attributes;
		}
	}
}
