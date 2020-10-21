using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Variables {
	public class UniformInfo {
		public int ProgramHandle { get; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public int Location { get; }
		public int Size { get; }
		public ActiveUniformType Type { get; }
		public UniformInfo(int ProgramHandle, string Name, int Location, int UniformSize, ActiveUniformType UniformType, bool Active) {
			this.ProgramHandle = ProgramHandle;
			this.Name = Name;
			this.Active = Active;
			this.Location = Location;
			this.Size = UniformSize;
			this.Type = UniformType;
		}
	}
}
