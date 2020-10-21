using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Variables {
	public class VertexAttributeInfo {
        public int ProgramHandle { get; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Index { get; private set; }
        public int Components { get; private set; }
        public ActiveAttribType Type { get; private set; }
        public bool Normalized { get; private set; }

        public VertexAttributeInfo(int ProgramHandle, string Name, bool Active, int Index, int Components, ActiveAttribType Type, bool Normalized) {
            this.ProgramHandle = ProgramHandle;
            this.Name = Name;
            this.Active = Active;
            this.Index = Index;
            this.Components = Components;
            this.Type = Type;
            this.Normalized = Normalized;
		}
    }

}
