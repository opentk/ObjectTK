using ObjectTK.Buffers;
using ObjectTK.Shaders.Variables;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectTK.Tools.Shapes {
	public class DynamicShape : Shape {

		private Action<VertexArray> DrawFunction;
		private Action DisposeFunction;

		public override void Draw() {
			base.Draw();
			DrawFunction?.Invoke(_VAO);
		}

		protected override void Dispose(bool manual) {
			if (!manual) return;
			base.Dispose(manual);
			DisposeFunction?.Invoke();
		}

		public DynamicShape WithVertexAttrib<T>(VertexAttrib Attrib, Buffer<T> Buffer) where T : struct {
			_VAO.BindAttribute(Attrib, Buffer);
			return this;
		}

		public DynamicShape WithElementBuffer(Buffer<uint> IndexBuffer) {
			_VAO.BindElementBuffer(IndexBuffer);
			return this;
		}

		/// <summary>
		/// Specifying a dispose function will *append* to any existing dispose function that is on this shape.
		/// </summary>
		/// <see cref="ShapeBuilder.CreateBasicCube(VertexAttrib)"/>
		/// <see cref="ShapeBuilder.CreateColoredCube(VertexAttrib, VertexAttrib)"/>
		/// <remarks>
		/// In the 2 functions listed above you can
		/// see that the ColoredCube only needs to dispose the color buffer
		/// because the other buffers are handled by the BasicCube function.
		/// </remarks>
		/// <param name="Action">The draw function</param>
		/// <returns></returns>
		public DynamicShape WithDisposeFunction(Action Action) {
			DisposeFunction += Action;
			return this;
		}

		/// <summary>
		/// Specifying a draw function will overwrite any existing draw function currently on this shape
		/// </summary>
		/// <param name="Action">The draw function</param>
		/// <returns></returns>
		public DynamicShape SetDrawFunction(Action<VertexArray> Action) {
			DrawFunction = Action;
			return this;
		}

	}


}
