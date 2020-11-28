using JetBrains.Annotations;
using ObjectTK.GLObjects;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Buffers {
	public static class VertexArrayExtensions {
		
		
		public static void Bind([NotNull] this VertexArray vertexArray) {
			GL.BindVertexArray(vertexArray.Handle);
		}
		
		public static void Unbind([NotNull] this VertexArray vertexArray) {
			GL.BindVertexArray(0);
		}

		public static void BindVertexAttribute<T>(this VertexArray vertexArray, ShaderAttributeInfo shaderAttributeInfo, Buffer<T> Buffer) where T : unmanaged {
			vertexArray.Bind();
			GL.VertexAttribPointer(shaderAttributeInfo.Location, Buffer.ElementCount, VertexAttribPointerType.Float, false, Buffer.ElementSize, 0);
			GL.EnableVertexAttribArray(shaderAttributeInfo.Location);
			GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer.Handle);
		}

	}
}
