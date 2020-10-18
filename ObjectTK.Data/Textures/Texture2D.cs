using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture2D : Texture {
		public override TextureTarget TextureTarget => TextureTarget.Texture2D;
		public int Width { get; set; }
		public int Height { get; set; }
		public int MipLevels { get; set; }
		public Texture2D(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int MipLevels) : base(Handle, InternalFormat) =>
			(this.Width, this.Height, this.MipLevels) = (Width, Height, MipLevels);
	}
}
