using ObjectTK.Data.Shaders;
using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Shaders {

	public static class ProgramExtensions {

		public static void Use(this Program Program) {
			GL.UseProgram(Program.Handle);
		}

	}

}
