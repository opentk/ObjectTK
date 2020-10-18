using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture2DArray : Texture2D {
		public override TextureTarget TextureTarget => TextureTarget.Texture2DArray;
		public int Layers { get; set; }
		public Texture2DArray(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int MipLevels, int Layers) : base(Handle, InternalFormat, Width, Height, MipLevels) =>
			this.Layers = Layers;
	}
}
