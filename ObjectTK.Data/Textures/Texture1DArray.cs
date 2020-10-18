using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture1DArray : Texture1D {
		public override TextureTarget TextureTarget => TextureTarget.Texture1DArray;
		public int Layers { get; set; }
		public Texture1DArray(int Handle, SizedInternalFormat InternalFormat, int Width, int Layers, int MipLevels) : base(Handle, InternalFormat, Width, MipLevels) =>
			this.Layers = Layers;
	}
}
