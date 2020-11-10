using ObjectTK.GLObjects;
using ObjectTK.Shaders;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Shaders {

	public static class ShaderFactory {
		public static ShaderStage CreateVertexShader(string Source) {
			int handle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(handle, Source);
			GL.CompileShader(handle);
			return new ShaderStage(ShaderType.VertexShader, handle, Source);
		}
		public static ShaderStage CreateFragmentShader(string Source) {
			int handle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(handle, Source);
			GL.CompileShader(handle);
			return new ShaderStage(ShaderType.FragmentShader, handle, Source);
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
