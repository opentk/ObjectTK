using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using ObjectTK.Extensions.Variables;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectTK.Extensions.Shaders {
	public class ProgramFactory {

		public string BaseDirectory { get; set; } = "./";
		public string ShaderExtension { get; set; } = "glsl";

		public ShaderProgram<T> CreateProgram<T>() where T : class, new() {

			List<ShaderSourceAttribute> Attributes = typeof(T).GetCustomAttributes<ShaderSourceAttribute>(true).ToList();
			List<ShaderStage> Shaders = new List<ShaderStage>();

			int ProgramHandle = GL.CreateProgram();

			foreach (ShaderSourceAttribute Attribute in Attributes) {
				string Source = GetEffectSource(Attribute.EffectKey);

				int ShaderHandle = GL.CreateShader(Attribute.Type);
				GL.ShaderSource(ShaderHandle, Source);
				GL.CompileShader(ShaderHandle);
				GL.AttachShader(ProgramHandle, ShaderHandle);

				switch (Attribute.Type) {
					case ShaderType.FragmentShader:
						Shaders.Add(new FragmentShaderStage(ShaderHandle, Source));
						break;
					case ShaderType.VertexShader:
						Shaders.Add(new VertexShaderStage(ShaderHandle, Source));
						break;
					default:
						break;
				}
			}

			GL.LinkProgram(ProgramHandle);

			foreach (ShaderStage Shader in Shaders) {
				GL.DetachShader(ProgramHandle, Shader.Handle);
				GL.DeleteShader(Shader.Handle);
			}

			ShaderProgram<T> ShaderProgram = new ShaderProgram<T>(ProgramHandle, null, null, new Dictionary<string, UniformInfo>(), new Dictionary<string, VertexAttributeInfo>());
			InitializeProgramVariables(ShaderProgram);

			return ShaderProgram;
		}

		private string GetEffectSource(string EffectKey, Dictionary<string, string> Effects = null) {
			
			Effects ??= new Dictionary<string, string>();

			string EffectFile = Path.ChangeExtension(EffectKey, null);
			string FilePath = Path.Join(BaseDirectory, Path.ChangeExtension(EffectKey, "glsl"));
			string FullSource = File.ReadAllText(FilePath);

			ExtractEffects(EffectFile, FullSource, Effects);

			Effects.TryGetValue(EffectKey, out string EffectSource);
			return EffectSource ?? throw new Exception($"Error loading effect with key: {EffectKey}");
		}
		private void ExtractEffects(string EffectFile, string FullSource, Dictionary<string, string> Effects) {

			const string EffectToken = "--";
			const string EffectFileSeparator = ".";
			const string IncludeToken = "#include";

			string[] Lines = FullSource.Split('\n');

			StringBuilder Effect = new StringBuilder();
			string EffectKey = null;

			foreach(string Line in Lines) {
				if (Line.StartsWith(EffectToken) && Line.Length > EffectToken.Length) {
					if(EffectKey != null) {
						Effects.Add(EffectKey, Effect.ToString());
						Effect.Clear();
						EffectKey = null;
					}
					EffectKey = $"{EffectFile}{EffectFileSeparator}{Line.Substring(EffectToken.Length).Trim()}";
					continue;
				}

				if(EffectKey != null) {
					if(Line.StartsWith(IncludeToken) && Line.Length > IncludeToken.Length) {
						string IncludedEffect = Line.Substring(IncludeToken.Length).Trim();
						if(!Effects.TryGetValue(IncludedEffect, out string EffectSource)) {
							EffectSource = GetEffectSource(IncludedEffect, Effects);
						}
						Effect.AppendLine(EffectSource);
					} else {
						Effect.AppendLine(Line);
					}
				}
			}
			if (EffectKey != null) {
				Effects.Add(EffectKey, Effect.ToString());
				Effect.Clear();
			}
		}

		private void InitializeProgramVariables<T>(ShaderProgram<T> shaderProgram) where T : class, new() {

			shaderProgram.Variables = new T();

			PropertyInfo[] Properties = typeof(T).GetProperties();
			shaderProgram.UniformInfoProperties = Properties.Where(Prop => typeof(UniformInfo).IsAssignableFrom(Prop.PropertyType)).ToList();
			shaderProgram.VertexAttributeInfoProperties = Properties.Where(Prop => typeof(VertexAttributeInfo).IsAssignableFrom(Prop.PropertyType)).ToList();

			foreach (PropertyInfo Prop in shaderProgram.UniformInfoProperties) {
				int UniformLocation = GL.GetUniformLocation(shaderProgram.Handle, Prop.Name);
				GL.GetActiveUniform(shaderProgram.Handle, UniformLocation, out int UniformSize, out ActiveUniformType UniformType);
				UniformInfo Value = Activator.CreateInstance(Prop.PropertyType, shaderProgram.Handle, Prop.Name, UniformLocation, UniformSize, UniformType, UniformLocation > -1) as UniformInfo;
				Prop.SetValue(shaderProgram.Variables, Value);
				shaderProgram.Uniforms.Add(Prop.Name, Value);
			}

			foreach (PropertyInfo Prop in shaderProgram.VertexAttributeInfoProperties) {
				int AttribIndex = GL.GetAttribLocation(shaderProgram.Handle, Prop.Name);
				GL.GetActiveAttrib(shaderProgram.Handle, AttribIndex, out int Size, out ActiveAttribType AttribType);
				VertexAttribAttribute Attribute = Prop.GetCustomAttribute<VertexAttribAttribute>();
				if(Attribute == null) {
					throw new Exception($"VertexAttributeInfo {typeof(T).FullName}.{Prop.Name} is not decorated with the 'VertexAttrib' Attribute, which is necessary for some metadata as it cannot be determined by the shader itself.");
				}
				VertexAttribPointerType VertexAttribPointerType = Attribute.VertexAttribPointerType;
				VertexAttributeInfo Value = Activator.CreateInstance(Prop.PropertyType, shaderProgram.Handle, Prop.Name, AttribIndex > -1, AttribIndex, Size, AttribType, VertexAttribPointerType, false) as VertexAttributeInfo;
				Prop.SetValue(shaderProgram.Variables, Value);
				shaderProgram.VertexAttributes.Add(Prop.Name, Value);
			}

		}
	}

}
