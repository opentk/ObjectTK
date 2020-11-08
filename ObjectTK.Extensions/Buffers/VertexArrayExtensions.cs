using ObjectTK.Data.Buffers;
using OpenTK.Graphics.OpenGL;
using ObjectTK.Data.Variables;

namespace ObjectTK.Extensions.Buffers {
	public static class VertexArrayExtensions {

		public static void Bind(this VertexArrayObject vertexArrayObject) {
			GL.BindVertexArray(vertexArrayObject.Handle);
		}

		public static void BindVertexAttribute<T>(this VertexArrayObject vertexArrayObject, ShaderAttributeInfo shaderAttributeInfo, Buffer<T> Buffer) where T : unmanaged {
			vertexArrayObject.Bind();
			GL.VertexAttribPointer(shaderAttributeInfo.Location, Buffer.ElementCount, VertexAttribPointerType.Float, false, Buffer.ElementSize, 0);
			GL.EnableVertexAttribArray(shaderAttributeInfo.Location);
			GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer.Handle);
		}

	}
}
