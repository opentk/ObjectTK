using Buffer = ObjectTK.Data.Buffers.Buffer;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Buffers {
	public static class BufferExtensions {

		public static void BufferData<T>(this Buffer Buffer, BufferTarget BufferTarget, T[] Data, BufferUsageHint BufferUsageHint = BufferUsageHint.StaticDraw) where T : struct {
			Buffer.ElementCount = Data.Length;
			GL.BindBuffer(BufferTarget, Buffer.Handle);
			GL.BufferData(BufferTarget, Buffer.ElementSize * Buffer.ElementCount, Data, BufferUsageHint);
		}


	}
}
