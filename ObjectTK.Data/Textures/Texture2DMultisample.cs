using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Data.Textures {
	public class Texture2DMultisample : Texture {
        public override TextureTarget TextureTarget => TextureTarget.Texture2DMultisample;
        public int Width { get; set; }
        public int Height { get; set; }
        public int Samples { get; set; }
        public bool FixedSampleLocations { get; set; }
        public Texture2DMultisample(int Handle, SizedInternalFormat InternalFormat, int Width, int Height, int Samples, bool FixedSampleLocations) : base(Handle, InternalFormat) =>
            (this.Width, this.Height, this.Samples, this.FixedSampleLocations) = (Width, Height, Samples, FixedSampleLocations);
    }
}
