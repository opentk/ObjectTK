using Buffer = ObjectTK.Data.Buffers.Buffer;
using ObjectTK.Data.Buffers;
using OpenTK.Graphics.OpenGL;
using ObjectTK.Data.Variables;

namespace ObjectTK.Extensions.Buffers {
	public static class VertexArrayExtensions {

		public static void Bind(this VertexArrayObject vertexArrayObject) {
			GL.BindVertexArray(vertexArrayObject.Handle);
		}

		public static void BindVertexAttribute(this VertexArrayObject vertexArrayObject, VertexAttributeInfo VertexAttributeInfo, Buffer Buffer) {
			vertexArrayObject.Bind();
			GL.VertexAttribPointer(VertexAttributeInfo.Index, Buffer.ElementCount, VertexAttribPointerType.Float, false, Buffer.ElementSize, 0);
			GL.EnableVertexAttribArray(VertexAttributeInfo.Index);
			GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer.Handle);
		}

	}
}
