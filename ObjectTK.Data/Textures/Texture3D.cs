using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture3D : Texture {
		public override TextureTarget TextureTarget => TextureTarget.Texture3D;
		int Width { get; set; }
		int Height { get; set; }
		int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture3D(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int MipLevels) : base(Handle, InternalFormat) =>
			(this.Width, this.Height, this.Layers, this.MipLevels) = (Width, Height, Layers, MipLevels);
	}
}
