using JetBrains.Annotations;

namespace ObjectTK.GLObjects {
	
	/// An OpenGL Vertex array object.
	public class VertexArray {
		
		/// The name of this object.
		public string Name { get; }
		
		/// The OpenGL handle. Use this to interact with OpenGL.
		public int Handle { get; }

		/// The buffers associated with this vertex array.
		[NotNull]
		[ItemNotNull]
		public Buffer[] Buffers { get; set; }

		
		/// The element array buffer for this object.
		/// If this is present, the vertex array should be displayed using indexed drawing.
		[CanBeNull]
		public Buffer IndexBuffer { get; set; }
		
		
		/// The number of elements to draw. If there is an element buffer, this is the length.
		/// If there is no element buffer, this is the length of the first buffer in the <see cref="Buffers"/> array.
		public int ElementCount {
			get
			{
				if (IndexBuffer != null) {
					return IndexBuffer.ElementCount;
				}
				
				return Buffers[0].ElementCount;
			}
		}

		public VertexArray(string name, int handle, [NotNull] Buffer[] buffers, [CanBeNull] Buffer indexBuffer) {
			Name = name;
			Handle = handle;
			Buffers = buffers;
			IndexBuffer = indexBuffer;
		}
	}
}
