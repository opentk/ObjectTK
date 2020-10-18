using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public interface ITexture {
		public int Handle { get; }
		TextureTarget TextureTarget { get; }
		SizedInternalFormat InternalFormat { get; set; }
	}
}
