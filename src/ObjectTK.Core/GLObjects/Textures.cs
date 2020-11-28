using OpenTK.Graphics.OpenGL;

namespace ObjectTK.GLObjects {
	public interface ITexture {
		int Handle { get; }
		string Name { get; set; }
		TextureTarget TextureTarget { get; }
		PixelInternalFormat InternalFormat { get; set; }
	}

	public class Texture1D : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture1D;
		public int Width { get; set; }
		public int MipLevels { get; set; }
		public Texture1D(int handle, string name, PixelInternalFormat internalFormat, int width, int mipLevels) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			MipLevels = mipLevels;
		}
	}

	public class Texture2D : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2D;
		public PixelInternalFormat InternalFormat { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		
		/// The ratio of Width/Height
		public float AspectRatio => (float) Width / Height;
		
		public Texture2D(int handle, string name, PixelInternalFormat internalFormat, int width, int height) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			Height = height;
		}
	}

	public class Texture1DArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture1DArray;
		public int Width { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture1DArray(int handle, string name, PixelInternalFormat internalFormat, int width, int layers, int mipLevels) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			Layers = layers;
			MipLevels = mipLevels;
		}
	}

	public class Texture2DArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DArray;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture2DArray(int handle, string name, PixelInternalFormat internalFormat, int width, int height, int layers, int mipLevels) {
			this.Handle = handle;
			this.Name = name;
			this.InternalFormat = internalFormat;
			this.Width = width;
			this.Height = height;
			this.Layers = layers;
			this.MipLevels = mipLevels;
		}
	}

	public class Texture2DMultisample : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DMultisample;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public bool FixedSampleLocations { get; set; }
		public Texture2DMultisample(int handle, string name, PixelInternalFormat internalFormat, int width, int height, int samples, bool fixedSampleLocations) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			Height = height;
			Samples = samples;
			FixedSampleLocations = fixedSampleLocations;
		}
	}

	public class Texture2DMultisampleArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture2DMultisampleArray;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Samples { get; set; }
		public bool FixedSampleLocations { get; set; }
		public int Layers { get; set; }
		public Texture2DMultisampleArray(int handle, string name, PixelInternalFormat internalFormat, int width, int height, int layers, int samples, bool fixedSampleLocations) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			Height = height;
			Layers = layers;
			Samples = samples;
			FixedSampleLocations = fixedSampleLocations;
		}
	}

	public class TextureRectangle : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureRectangle;
		public int Width { get; set; }
		public int Height { get; set; }
		public TextureRectangle(int handle, string name, PixelInternalFormat internalFormat, int width, int height) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			Height = height;
		}
	}

	public class TextureCubeMap : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureCubeMap;
		public int Size { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMap(int handle, string name, PixelInternalFormat internalFormat, int size, int mipLevels) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Size = size;
			MipLevels = mipLevels;
		}
	}

	public sealed class TextureCubeMapArray : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureCubeMapArray;
		public int Size { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public TextureCubeMapArray(int handle, string name, PixelInternalFormat internalFormat, int size, int layers, int mipLevels) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Size = size;
			Layers = layers;
			MipLevels = mipLevels;
		}
	}

	public class TextureBuffer : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.TextureBuffer;
		//TODO: I don't know much about this texture type
		public TextureBuffer(int handle, string name, PixelInternalFormat internalFormat) {
			Handle = handle;
			InternalFormat = internalFormat;
		}
	}

	public class Texture3D : ITexture {
		public int Handle { get; }
		public string Name { get; set; }
		public PixelInternalFormat InternalFormat { get; set; }
		public TextureTarget TextureTarget => TextureTarget.Texture3D;
		public int Width { get; set; }
		public int Height { get; set; }
		public int Layers { get; set; }
		public int MipLevels { get; set; }
		public Texture3D(int handle, string name, PixelInternalFormat internalFormat, int width, int height, int layers, int mipLevels) {
			Handle = handle;
			Name = name;
			InternalFormat = internalFormat;
			Width = width;
			Height = height;
			Layers = layers;
			MipLevels = mipLevels;
		}
	}
}
