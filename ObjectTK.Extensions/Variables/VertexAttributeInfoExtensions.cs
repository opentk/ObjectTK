using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;
using System;

namespace ObjectTK.Extensions.Variables {
	public static class VertexAttributeInfoExtensions {


	}
	//
	// public class ShaderAttributeInfo<T> : ShaderAttributeInfo {
	// 	public ShaderAttributeInfo(int ProgramHandle, string Name, bool Active, int Index, int Components, ActiveAttribType ActiveAttribType, VertexAttribPointerType VertexAttribPointerType, bool Normalized) :
	// 		base(Name, Active, Index, Components, ActiveAttribType, VertexAttribPointerType, Normalized) {
	// 	}
	// }

	[AttributeUsage(AttributeTargets.Property)]
	public class VertexAttribAttribute : Attribute {
		public VertexAttribPointerType VertexAttribPointerType { get; protected set; }
		public bool Normalized { get; protected set; }

		/// <summary>
		/// Defines some metadata for VertexAttributeInfo objects that cannot be gathered from OpenGL.
		/// </summary>
		public VertexAttribAttribute(VertexAttribPointerType VertexAttribPointerType = VertexAttribPointerType.Float, bool Normalized = false) {
			this.VertexAttribPointerType = VertexAttribPointerType;
			this.Normalized = Normalized;
		}
	}

}
