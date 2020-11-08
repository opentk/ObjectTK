using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Variables {
	public class ShaderAttributeInfo {
		public string Name { get; set; }
		public int Location { get; set; }
		/// The size of the attribute, in units of the attribute type.
		public int Size { get; set; }
		/// 
		public ActiveAttribType ActiveAttribType { get; set; }

		public VertexAttribPointerType VertexAttribPointerType => ActiveAttribType.ToVertexAttribPointerType();

		//
		// public ShaderAttributeInfo(string name, int index, int components, ActiveAttribType activeAttribType, VertexAttribPointerType vertexAttribPointerType, bool normalized) {
		// 	this.Name = name;
		// 	this.Index = index;
		// 	this.Components = components;
		// 	this.ActiveAttribType = activeAttribType;
		// 	this.VertexAttribPointerType = vertexAttribPointerType;
		// 	this.Normalized = normalized;
		// }
	}

}
