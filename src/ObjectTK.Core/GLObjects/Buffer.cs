using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ObjectTK.GLObjects {

	/// Corresponds to an OpenGL Buffer.
	/// Typically this is a vertex/index/whatever buffer.
	public class Buffer {
		/// The name of this buffer
		public string Name { get; set; }
		/// The OpenGL Handle for this.
		public int Handle { get; }
		/// The GL data type (float, int, etc). (Vector2 -> float)
		public VertexAttribPointerType AttribType { get; }
		/// The number of components in each element. (Vector2 -> 2)
		public int ComponentCount { get; }
		/// Size in bytes of each element in the buffer (Vector2 -> 8 bytes)
		public int ElementSize { get; }
		/// The number of elements in this buffer. (based on the data set in this buffer)
		public int ElementCount { get; set; }

		public Buffer(string name, int handle, VertexAttribPointerType attribType, int componentCount, int elementSize, int elementCount) {
			Name = name;
			Handle = handle;
			AttribType = attribType;
			ComponentCount = componentCount;
			ElementSize = elementSize;
			ElementCount = elementCount;
		}
	}
	
	internal static class BufferHelper {
		internal struct TypeToGLInfoCache {
			public VertexAttribPointerType AttribPointerType;
			public int ComponentCount;

			public TypeToGLInfoCache(VertexAttribPointerType attribPointerType, int componentCount) {
				AttribPointerType = attribPointerType;
				ComponentCount = componentCount;
			}
		}
		
		private static readonly Dictionary<Type, TypeToGLInfoCache> InfoCache = new Dictionary<Type, TypeToGLInfoCache>();

		private static void Add<T2>(VertexAttribPointerType vapt, int componentCount) {
			InfoCache[typeof(T2)] = new TypeToGLInfoCache(vapt, componentCount);
		}
		
		static BufferHelper() {
			Add<byte>(VertexAttribPointerType.Byte, 1);
			Add<sbyte>(VertexAttribPointerType.UnsignedByte, 1);

			Add<Color>(VertexAttribPointerType.UnsignedByte, 4);

			Add<short>(VertexAttribPointerType.Short, 1);
			Add<ushort>(VertexAttribPointerType.UnsignedShort, 1);
			
			Add<uint>(VertexAttribPointerType.UnsignedInt, 1);
			
			Add<int>(VertexAttribPointerType.Int, 1);
			Add<Vector2i>(VertexAttribPointerType.Int, 2);
			Add<Vector3i>(VertexAttribPointerType.Int, 3);
			Add<Vector4i>(VertexAttribPointerType.Int, 4);

			Add<Half>(VertexAttribPointerType.HalfFloat, 1);
			Add<Vector2h>(VertexAttribPointerType.HalfFloat, 2);
			Add<Vector3h>(VertexAttribPointerType.HalfFloat, 3);
			Add<Vector4h>(VertexAttribPointerType.HalfFloat, 4);
			
			Add<float>(VertexAttribPointerType.Float, 1);
			Add<Vector2>(VertexAttribPointerType.Float, 2);
			Add<Vector3>(VertexAttribPointerType.Float, 3);
			Add<Vector4>(VertexAttribPointerType.Float, 4);
			Add<Color4>(VertexAttribPointerType.Float, 4);
			
			Add<double>(VertexAttribPointerType.Double, 1);
			Add<Vector2d>(VertexAttribPointerType.Double, 2);
			Add<Vector3d>(VertexAttribPointerType.Double, 3);
			Add<Vector4d>(VertexAttribPointerType.Double, 4);

			Add<Quaternion>(VertexAttribPointerType.Float, 4);
			Add<Quaterniond>(VertexAttribPointerType.Double, 4);
			
			// numerics

			Add<System.Numerics.Vector2>(VertexAttribPointerType.Float, 2);
			Add<System.Numerics.Vector3>(VertexAttribPointerType.Float, 3);
			Add<System.Numerics.Vector4>(VertexAttribPointerType.Float, 4);
			
		}

		internal static TypeToGLInfoCache GetData<T>() {
			return InfoCache[typeof(T)];
		}
	}
	
	/// Corresponds to an OpenGL Buffer.
	/// Typically this is a vertex/index/whatever buffer.
	/// Strongly typed buffer. This can be upcast to a normal buffer if generic usage is required.
	public class Buffer<T> : Buffer where T : unmanaged {


		public unsafe Buffer(string name, int handle, int elementCount) : base(name, handle, BufferHelper.GetData<T>().AttribPointerType,
			BufferHelper.GetData<T>().ComponentCount, sizeof(T), elementCount) {
			
		}
	}
}
