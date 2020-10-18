using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture2DMultisampleArray : Texture2DMultisample {
		public override TextureTarget TextureTarget => TextureTarget.Texture2DMultisampleArray;
		public int Layers { get; set; }
		public Texture2DMultisampleArray(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Samples, bool FixedSampleLocations, int Layers) :
			base(Handle, InternalFormat, Width, Height, Samples, FixedSampleLocations) =>
			this.Layers = Layers;
	}
}
