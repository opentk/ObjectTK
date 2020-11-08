using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using ObjectTK.Extensions.Variables;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Reflection;

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

		public ShaderProgram(int Handle, VertexShaderStage vertexShaderStage, FragmentShaderStage fragmentShaderStage, Dictionary<string, UniformInfo> Uniforms, Dictionary<string, VertexAttributeInfo> VertexAttributes) :
			base(Handle, vertexShaderStage, fragmentShaderStage, Uniforms, VertexAttributes) {
		}
	}

	public class Material<T> where T : class, new() {
		public ShaderProgram<T> ShaderProgram { get; set; }
		public T Variables { get; set; }

		public Material() {

			foreach (PropertyInfo Prop in ShaderProgram.UniformInfoProperties) {
				Type UniformType = Prop.PropertyType.GetGenericArguments()[0];
				object DefaultValue = Activator.CreateInstance(UniformType);
				UniformInfo PropValue = Prop.GetValue(ShaderProgram.Variables) as UniformInfo;
			}
		}

		public void Use() {
			ShaderProgram.Use();
		}

	}

}
