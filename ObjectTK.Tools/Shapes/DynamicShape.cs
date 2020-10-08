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

		public DynamicShape WithDisposeFunction(Action Action) {
			DisposeFunction += Action;
			return this;
		}

		public DynamicShape WithDrawFunction(Action<VertexArray> Action) {
			DrawFunction += Action;
			return this;
		}

	}


}
