using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Shaders {
	public abstract class Shader {
		public int Handle { get; }
		public string Source { get; }
		public abstract ShaderType ShaderType { get; }

		public Shader(int Handle, string Source) {
			this.Handle = Handle;
			this.Source = Source;
		}
	}
	public class VertexShader : Shader {
		public override ShaderType ShaderType => ShaderType.VertexShader;
		public VertexShader(int Handle, string Source) : base(Handle, Source) {

		}
	}

	public class FragmentShader : Shader {
		public override ShaderType ShaderType => ShaderType.FragmentShader;
		public FragmentShader(int Handle, string Source) : base(Handle, Source) {

		}

	}
}
