using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public abstract class Texture : GLObject {
		public abstract TextureTarget TextureTarget { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public Texture(int Handle, SizedInternalFormat InternalFormat) : base(Handle) => this.InternalFormat = InternalFormat;
	}
}
