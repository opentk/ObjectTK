using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class TextureCubeMap : Texture {
		public override TextureTarget TextureTarget => TextureTarget.TextureCubeMap;
		public int Size { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMap(int Handle, SizedInternalFormat InternalFormat, int Size, int MipLevels) : base(Handle, InternalFormat) =>
			(this.Size, this.MipLevels) = (Size, MipLevels);
	}
}
