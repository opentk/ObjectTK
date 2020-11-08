using ObjectTK.Data.Shaders;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Shaders {

	public static class ShaderFactory {
		public static VertexShaderStage CreateVertexShader(string Source) {
			int Handle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(Handle, Source);
			GL.CompileShader(Handle);
			return new VertexShaderStage(Handle, Source);
		}
		public static FragmentShaderStage CreateFragmentShader(string Source) {
			int Handle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(Handle, Source);
			GL.CompileShader(Handle);
			return new FragmentShaderStage(Handle, Source);
		}
	}

	public static class ShaderExtensions {
		public static void Compile(this ShaderStage shaderStage) {
			GL.CompileShader(shaderStage.Handle);
		}
		public static void ShaderSource(this ShaderStage shaderStage) {
			GL.ShaderSource(shaderStage.Handle, shaderStage.Source);
		}
	}
}
