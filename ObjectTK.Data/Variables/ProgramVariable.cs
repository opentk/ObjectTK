using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Variables {
	public class ProgramVariable {

		public int ProgramHandle { get; }
		public string Name { get; set; }
		public bool Active { get; set; }

	}
}
