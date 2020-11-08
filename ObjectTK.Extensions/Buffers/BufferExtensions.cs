using ObjectTK.Data.Buffers;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Buffers {
	public static class BufferExtensions {

		public static void BufferData<T>(this Buffer<T> Buffer, BufferTarget BufferTarget, T[] Data, BufferUsageHint BufferUsageHint = BufferUsageHint.StaticDraw) where T : unmanaged {
			Buffer.ElementCount = Data.Length;
			GL.BindBuffer(BufferTarget, Buffer.Handle);
			GL.BufferData(BufferTarget, Buffer.ElementSize * Buffer.ElementCount, Data, BufferUsageHint);
		}


	}
}
