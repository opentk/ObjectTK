using ObjectTK.Data.Shaders;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Shaders {

	public static class ShaderFactory {
		public static VertexShader CreateVertexShader(string Source) {
			int Handle = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(Handle, Source);
			GL.CompileShader(Handle);
			return new VertexShader(Handle, Source);
		}
		public static FragmentShader CreateFragmentShader(string Source) {
			int Handle = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(Handle, Source);
			GL.CompileShader(Handle);
			return new FragmentShader(Handle, Source);
		}
	}

	public static class ShaderExtensions {
		public static void Compile(this Shader Shader) {
			GL.CompileShader(Shader.Handle);
		}
		public static void ShaderSource(this Shader Shader) {
			GL.ShaderSource(Shader.Handle, Shader.Source);
		}
	}
}
