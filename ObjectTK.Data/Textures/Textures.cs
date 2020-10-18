using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public interface ITexture {
		int Handle { get; }
		string Name { get; set; }
		TextureTarget TextureTarget { get; }
		SizedInternalFormat InternalFormat { get; set; }
	}

	public sealed class Texture1D : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture1D;
		public int Width { get; set; }
		public int MipLevels { get; set; }
		public Texture1D(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.MipLevels = MipLevels;
		}

	}

	public sealed class Texture2D : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2D;
		public int Width { get; set; }
		public int Height { get; set; }
		public int MipLevels { get; set; }
		public Texture2D(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Height, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Height = Height;
			this.MipLevels = MipLevels;
		}
	}

	public sealed class Texture1DArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture1DArray;
		public int Width { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture1DArray(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Layers, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Layers = Layers;
			this.MipLevels = MipLevels;
		}
	}

	public sealed class Texture2DArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DArray;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture2DArray(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Height = Height;
			this.Layers = Layers;
			this.MipLevels = MipLevels;
		}
	}

	public sealed class Texture2DMultisample : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DMultisample;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public bool FixedSampleLocations { get; set; }
		public Texture2DMultisample(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Height, int Samples, bool FixedSampleLocations) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Height = Height;
			this.Samples = Samples;
			this.FixedSampleLocations = FixedSampleLocations;
		}
	}

	public sealed class Texture2DMultisampleArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DMultisampleArray;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public bool FixedSampleLocations { get; set; }
		public int Layers { get; set; }
		public Texture2DMultisampleArray(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int Samples, bool FixedSampleLocations) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Height = Height;
			this.Layers = Layers;
			this.Samples = Samples;
			this.FixedSampleLocations = FixedSampleLocations;
		}
	}

	public sealed class TextureRectangle : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureRectangle;
		public int Width { get; set; }
		public int Height { get; set; }
		public TextureRectangle(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Height) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Height = Height;
		}
	}

	public sealed class TextureCubeMap : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureCubeMap;
		public int Size { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMap(int Handle, string Name, SizedInternalFormat InternalFormat, int Size, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Size = Size;
			this.MipLevels = MipLevels;
		}
	}

	public sealed class TextureCubeMapArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureCubeMapArray;
		public int Size { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMapArray(int Handle, string Name, SizedInternalFormat InternalFormat, int Size, int Layers, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Size = Size;
			this.Layers = Layers;
			this.MipLevels = MipLevels;
		}
	}

	public sealed class TextureBuffer : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureBuffer;
		//TODO: I don't know much about this texture type
		public TextureBuffer(int Handle, string Name, SizedInternalFormat InternalFormat) {
			this.Handle = Handle;
			this.InternalFormat = InternalFormat;
		}
	}

	public sealed class Texture3D : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public SizedInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture3D;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture3D(int Handle, string Name, SizedInternalFormat InternalFormat, int Width, int Height, int Layers, int MipLevels) {
			this.Handle = Handle;
			this.Name = Name;
			this.InternalFormat = InternalFormat;
			this.Width = Width;
			this.Height = Height;
			this.Layers = Layers;
			this.MipLevels = MipLevels;
		}
	}
}