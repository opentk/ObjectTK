using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class TextureRectangle : Texture {
		public override TextureTarget TextureTarget => TextureTarget.TextureRectangle;
		public int Width { get; set; }
		public int Height { get; set; }
		public TextureRectangle(int Handle, SizedInternalFormat InternalFormat, int Width, int Height) : base(Handle, InternalFormat) =>
			(this.Width, this.Height) = (Width, Height);
	}
}
