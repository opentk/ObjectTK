using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class TextureBuffer : Texture {
		public override TextureTarget TextureTarget => TextureTarget.TextureBuffer;
		public TextureBuffer(int Handle, SizedInternalFormat InternalFormat) : base(Handle, InternalFormat) {
			//TODO: I don't know much about this texture type
		}
	}
}
