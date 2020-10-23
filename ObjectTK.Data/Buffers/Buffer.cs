using System.Runtime.InteropServices;

namespace ObjectTK.Data.Buffers {

	public class Buffer {
		public int Handle { get; }
		public int ElementSize { get; }
		private int _ElementCount { get; set; }
		public virtual int ElementCount {
			get {
				return _ElementCount;
			}
			set {
				_ElementCount = value;
			}
		}

		public Buffer(int Handle, int ElementSize, int ElementCount) {
			this.Handle = Handle;
			this.ElementSize = ElementSize;
			this.ElementCount = ElementCount;
		}
	}

	public class Buffer<T> : Buffer where T : struct {
		public Buffer(int Handle) : base(Handle, Marshal.SizeOf(typeof(T)), 0) {

		}
	}
}
