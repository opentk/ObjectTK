namespace ObjectTK.GLObjects {
	
	/// An OpenGL Vertex array object.
	public class VertexArray {
		
		/// The name of this object.
		public string Name { get; }
		/// The OpenGL handle. Use this to interact with OpenGL.
		public int Handle { get; }
		/// The number of elements in this VAO.
		public int ElementCount { get; set; }
		
		/// If this vertex array has an element array buffer (i.e. should be displayed using indexed drawing).
		public bool HasElementArrayBuffer { get; set; }

		public VertexArray(string name, int handle, int elementCount, bool hasElementArrayBuffer) {
			Name = name;
			Handle = handle;
			ElementCount = elementCount;
			HasElementArrayBuffer = hasElementArrayBuffer;
		}
	}
}
