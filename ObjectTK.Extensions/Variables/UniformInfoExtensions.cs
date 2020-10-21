using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Variables {
	public static class UniformInfoExtensions {

		//public static void Set<T>(this Uniform<T> Uniform , T Value) {

		//}

	}
	public class UniformInfo<T> : UniformInfo {
		public UniformInfo(int ProgramHandle, string Name, int Location, int UniformSize, ActiveUniformType UniformType, bool Active) :
			base(ProgramHandle, Name, Location, UniformSize, UniformType, Active) {
		}
	}
}
