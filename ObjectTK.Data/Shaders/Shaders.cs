using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Shaders {
	public abstract class ShaderStage {
		public int Handle { get; }
		public string Source { get; }
		public abstract ShaderType ShaderType { get; }

		public ShaderStage(int Handle, string Source) {
			this.Handle = Handle;
			this.Source = Source;
		}
	}
	public class VertexShaderStage : ShaderStage {
		public override ShaderType ShaderType => ShaderType.VertexShader;
		public VertexShaderStage(int Handle, string Source) : base(Handle, Source) {

		}
	}

	public class FragmentShaderStage : ShaderStage {
		public override ShaderType ShaderType => ShaderType.FragmentShader;
		public FragmentShaderStage(int Handle, string Source) : base(Handle, Source) {

		}

	}
}
