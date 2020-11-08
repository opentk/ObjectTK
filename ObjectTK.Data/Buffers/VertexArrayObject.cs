namespace ObjectTK.Data.Buffers {
	
	/// An OpenGL Vertex array object.
	public class VertexArrayObject {
		
		public int Handle { get; }

		public VertexArrayObject(int handle) {
			Handle = handle;
		}
	}
}
