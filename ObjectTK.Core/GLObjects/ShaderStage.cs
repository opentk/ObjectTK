using OpenTK.Graphics.OpenGL;

namespace ObjectTK.GLObjects {
	/// https://www.khronos.org/opengl/wiki/Shader#Stages
	public class ShaderStage {
		
		public string Name { get; }
		public ShaderType Type { get; }
		public int Handle { get; }
		public string Source { get; }

		public ShaderStage(ShaderType type, int handle, string source) {
			Type = type;
			Handle = handle;
			Source = source;
		}
	}
}
