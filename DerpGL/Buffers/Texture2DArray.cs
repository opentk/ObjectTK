using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    public class Texture2DArray
        : Texture
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Layers { get; private set; }

        public Texture2DArray(SizedInternalFormat internalFormat, int width, int height, int layers)
            : base(internalFormat)
        {
            Width = width;
            Height = height;
            Layers = layers;
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, internalFormat, width, height, layers);
            CheckError();
        }
    }
}