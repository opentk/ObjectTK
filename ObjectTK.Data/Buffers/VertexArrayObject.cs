namespace ObjectTK.Data.Buffers {
	
	/// An OpenGL Vertex array object.
	public class VertexArrayObject {
		
		/// The name of this object.
		public string Name { get; }
		/// The OpenGL handle. Use this to interact with OpenGL.
		public int Handle { get; }
		/// The number of elements in this VAO.
		public int ElementCount { get; }

		public VertexArrayObject(string name, int handle, int elementCount) {
			Name = name;
			Handle = handle;
			ElementCount = elementCount;
		}
	}
}
