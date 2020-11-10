using OpenTK.Graphics.OpenGL;

namespace ObjectTK.GLObjects {
	public class ShaderUniformInfo {
		//TODO FIXME: Name[] for array uniform variables can be challenging.
		/// The name of this uniform in the array
		public string Name { get; set; }
		/// The uniform location. used with GL.SetUniform()
		public int Location { get; }
		/// If this uniform is an array, then this is the number of items in the array.
		public int Size { get; }
		/// The data type in the shader for this uniform.
		public ActiveUniformType Type { get; }
		
		public ShaderUniformInfo(string name, int location, 
			int uniformSize, ActiveUniformType uniformType) {
			Name = name;
			Location = location;
			Size = uniformSize;
			Type = uniformType;
		}
	}
}
