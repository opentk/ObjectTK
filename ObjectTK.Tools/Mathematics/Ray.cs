using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectTK.Tools.Mathematics {
	public class Ray {
		public Vector3 Origin { get; set; }
		public Vector3 Direction { get; set; }
		public float Length { get; set; }
	}
}
