using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public sealed class Texture1D : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture1D;
		public int Width { get; set; }
		public int MipLevels { get; set; }
		public Texture1D(int Handle, SizedInternalFormat InternalFormat, int Width, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Width, this.MipLevels) = (Handle, InternalFormat, Width, MipLevels);
	}

	public sealed class Texture2D : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2D;
		public int Width { get; set; }
		public int Height { get; set; }
		public int MipLevels { get; set; }
		public Texture2D(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Width, this.Height, this.MipLevels) = (Handle, InternalFormat, Width, Height, MipLevels);
	}

	public sealed class Texture1DArray : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture1DArray;
		public int Width { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture1DArray(int Handle, SizedInternalFormat InternalFormat, int Width, int Layers, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Width, this.Layers, this.MipLevels) = (Handle, InternalFormat, Width, Layers, MipLevels);
	}

	public sealed class Texture2DArray : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DArray;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture2DArray(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Width, this.Height, this.Layers, this.MipLevels) = (Handle, InternalFormat, Width, Height, Layers, MipLevels);
	}

	public sealed class Texture2DMultisample : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DMultisample;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public bool FixedSampleLocations { get; set; }
		public Texture2DMultisample(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Samples, bool FixedSampleLocations) =>
			(this.Handle, this.InternalFormat, this.Width, this.Height, this.Samples, this.FixedSampleLocations) = (Handle, InternalFormat, Width, Height, Samples, FixedSampleLocations);
	}

	public sealed class Texture2DMultisampleArray : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DMultisampleArray;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public bool FixedSampleLocations { get; set; }
		public int Layers { get; set; }
		public Texture2DMultisampleArray(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int Samples, bool FixedSampleLocations) =>
			(this.Handle, this.InternalFormat, this.Width, this.Height, this.Layers, this.Samples, this.FixedSampleLocations) = (Handle, InternalFormat, Width, Height, Layers, Samples, FixedSampleLocations);
	}

	public sealed class TextureRectangle : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureRectangle;
		public int Width { get; set; }
		public int Height { get; set; }
		public TextureRectangle(int Handle, SizedInternalFormat InternalFormat, int Width, int Height) =>
			(this.Handle, this.InternalFormat, this.Width, this.Height) = (Handle, InternalFormat, Width, Height);
	}

	public sealed class TextureCubeMap : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureCubeMap;
		public int Size { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMap(int Handle, SizedInternalFormat InternalFormat, int Size, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Size, this.MipLevels) = (Handle, InternalFormat, Size, MipLevels);
	}

	public sealed class TextureCubeMapArray : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureCubeMapArray;
		public int Size { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMapArray(int Handle, SizedInternalFormat InternalFormat, int Size, int Layers, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Size, this.Layers, this.MipLevels) = (Handle, InternalFormat, Size, Layers, MipLevels);
	}

	public sealed class TextureBuffer : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureBuffer;
		//TODO: I don't know much about this texture type
		public TextureBuffer(int Handle, SizedInternalFormat InternalFormat) =>
			(this.Handle, this.InternalFormat) = (Handle, InternalFormat);
	}

	public sealed class Texture3D : ITexture {
		public int Handle { get; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture3D;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture3D(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int MipLevels) =>
			(this.Handle, this.InternalFormat, this.Width, this.Height, this.Layers, this.MipLevels) = (Handle, InternalFormat, Width, Height, Layers, MipLevels);
	}
}