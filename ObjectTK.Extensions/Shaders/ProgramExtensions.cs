using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using ObjectTK.Extensions.Variables;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectTK.Extensions.Shaders {

	public static class ProgramExtensions {

		public static void Use(this Program Program) {
			GL.UseProgram(Program.Handle);
		}

	}
	public class Program<T> : Program where T : class, new() {
		public T Variables { get; set; }
		internal List<PropertyInfo> UniformInfoProperties { get; set; }
		internal List<PropertyInfo> VertexAttributeInfoProperties { get; set; }

		public Program(int Handle, VertexShader VertexShader, FragmentShader FragmentShader, Dictionary<string, UniformInfo> Uniforms, Dictionary<string, VertexAttributeInfo> VertexAttributes) :
			base(Handle, VertexShader, FragmentShader, Uniforms, VertexAttributes) {
		}
	}

	public class Material<T> where T : class, new() {
		public Program<T> Program { get; set; }
		public T Variables { get; set; }

		public Material() {

			foreach (PropertyInfo Prop in Program.UniformInfoProperties) {
				Type UniformType = Prop.PropertyType.GetGenericArguments()[0];
				object DefaultValue = Activator.CreateInstance(UniformType);
				UniformInfo PropValue = Prop.GetValue(Program.Variables) as UniformInfo;
			}
		}

		public void Use() {
			Program.Use();
		}

	}

}
