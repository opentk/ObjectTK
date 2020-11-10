using ObjectTK.Extensions.Variables;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Reflection;
using ObjectTK.GLObjects;
using ObjectTK.Shaders;

namespace ObjectTK.Extensions.Shaders {

	public static class ProgramExtensions {

		public static void Use(this ShaderProgram shaderProgram) {
			GL.UseProgram(shaderProgram.Handle);
		}

	}
	public class ShaderProgram<T> : ShaderProgram where T : class, new() {
		public T Variables { get; set; }
		internal List<PropertyInfo> UniformInfoProperties { get; set; }
		internal List<PropertyInfo> VertexAttributeInfoProperties { get; set; }

		public ShaderProgram(int handle, ShaderStage[] stages,
			Dictionary<string, ShaderUniformInfo> uniforms,
			Dictionary<string, ShaderAttributeInfo> attributes) :
			base(handle, stages, uniforms, attributes) {
		}
	}
}
