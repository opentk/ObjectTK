using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.ES11.GL;

namespace ObjectTK.Data.Buffers {
	
	/// Corresponds to an OpenGL Buffer.
	/// Typically this is a vertex/index/whatever buffer.
	public class Buffer<T> where T : unmanaged {
		
		/// The OpenGL Handle for this.
		public int Handle { get; }
		/// Size in bytes of each element in the buffer.
		public int ElementSize { get; }
		/// The number of elements in this buffer.
		public int ElementCount { get; set; }

		public unsafe Buffer(int handle, int elementCount) {
			Handle = handle;
			ElementSize = sizeof(T);
			ElementCount = elementCount;
		}
	}
}
