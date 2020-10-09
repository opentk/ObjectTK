//
// Shape.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace ObjectTK.Tools.Shapes {
	public abstract class Shape : GLResource {
		public readonly VertexArray _VAO;

		public Shape() {
			_VAO = new VertexArray();
			_VAO.Bind();
		}

		public virtual void Draw() {
			_VAO.Bind();
		}

		protected override void Dispose(bool manual) {
			if (!manual) return;

			_VAO?.Dispose();
		}
	}

}
