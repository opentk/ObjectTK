using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public sealed class TextureCubeMapArray : TextureCubeMap {
		public override TextureTarget TextureTarget => TextureTarget.TextureCubeMapArray;
		public int Layers { get; set; }
		public TextureCubeMapArray(int Handle, SizedInternalFormat InternalFormat, int Size, int Layers, int MipLevels) : base(Handle, InternalFormat, Size, MipLevels) =>
			this.Layers = Layers;
	}
}
