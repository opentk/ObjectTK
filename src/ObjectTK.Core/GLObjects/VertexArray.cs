namespace ObjectTK.GLObjects {
	
	/// An OpenGL Vertex array object.
	public class VertexArray {
		
		/// The name of this object.
		public string Name { get; }
		/// The OpenGL handle. Use this to interact with OpenGL.
		public int Handle { get; }
		/// The number of elements in this VAO.
		public int ElementCount { get; set; }

		public VertexArray(string name, int handle, int elementCount) {
			Name = name;
			Handle = handle;
			ElementCount = elementCount;
		}
	}
}
