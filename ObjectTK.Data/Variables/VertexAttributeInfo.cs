using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Variables {
	public class VertexAttributeInfo {
		public int ProgramHandle { get; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public int Index { get; set; }
		public int Components { get; set; }
		public ActiveAttribType ActiveAttribType { get; set; }
		public VertexAttribPointerType VertexAttribPointerType { get; set; }
		public bool Normalized { get; set; }

		public VertexAttributeInfo(int ProgramHandle, string Name, bool Active, int Index, int Components, ActiveAttribType ActiveAttribType, VertexAttribPointerType VertexAttribPointerType, bool Normalized) {
			this.ProgramHandle = ProgramHandle;
			this.Name = Name;
			this.Active = Active;
			this.Index = Index;
			this.Components = Components;
			this.ActiveAttribType = ActiveAttribType;
			this.VertexAttribPointerType = VertexAttribPointerType;
			this.Normalized = Normalized;
		}
	}

}
