using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectTK.Data.Shaders {
	public class Program {
		public int Handle { get; }
		public VertexShader VertexShader { get; }
		public FragmentShader FragmentShader { get; }

		public Program(int Handle, VertexShader VertexShader, FragmentShader FragmentShader) {
			this.Handle = Handle;
			this.VertexShader = VertexShader;
			this.FragmentShader = FragmentShader;
		}

	}
}
