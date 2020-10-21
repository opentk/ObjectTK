using OpenTK.Graphics.OpenGL;
using System;

namespace ObjectTK.Extensions.Shaders {

	[AttributeUsage(AttributeTargets.Class)]
	public class ShaderSourceAttribute : Attribute {

		public ShaderType Type { get; private set; }

		public string EffectKey { get; private set; }

		public ShaderSourceAttribute(ShaderType Type, string EffectKey) {
			this.Type = Type;
			this.EffectKey = EffectKey;
		}

	}

	public class VertexShaderSourceAttribute : ShaderSourceAttribute {
		public VertexShaderSourceAttribute(string EffectKey) : base(ShaderType.VertexShader, EffectKey) {

		}
	}

	public class FragmentShaderSourceAttribute : ShaderSourceAttribute {
		public FragmentShaderSourceAttribute(string EffectKey) : base(ShaderType.FragmentShader, EffectKey) {

		}
	}

}
