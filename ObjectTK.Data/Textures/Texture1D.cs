using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture1D : Texture {
		public override TextureTarget TextureTarget => TextureTarget.Texture1D;
		public int Width { get; set; }
        public int MipLevels { get; set; }
        public Texture1D(int Handle, SizedInternalFormat InternalFormat, int Width, int MipLevels) : base(Handle, InternalFormat) =>
			(this.Width, this.MipLevels) = (Width, MipLevels);
	}
}