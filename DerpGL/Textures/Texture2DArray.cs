using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D texture array.
    /// </summary>
    public sealed class Texture2DArray
        : LayeredTexture
    {
        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The number of layers.
        /// </summary>
        public int Layers { get; private set; }

        /// <summary>
        /// Creates a 2D texture array with the given number of layers and internal format, width and height compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap which is used to determine compatible internal format, width and height of the texture array.</param>
        /// <param name="layers">The number of layers to allocate.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture2DArray(Bitmap bitmap, int layers, int levels)
            : this(FormatMapping.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height, layers, levels)
        {
            SetDefaultTexParameters();
        }

        /// <summary>
        /// Creates a 2D texture array with given internal format, width, height and number of layers.
        /// </summary>
        public Texture2DArray(SizedInternalFormat internalFormat, int width, int height, int layers)
            : base(TextureTarget.Texture2DArray, internalFormat)
        {
            Initialize(width, height, layers);
        }

        /// <summary>
        /// Creates a 2D texture array with given internal format, width, height, number of layers and number of mipmap levels.
        /// </summary>
        public Texture2DArray(SizedInternalFormat internalFormat, int width, int height, int layers, int levels)
            : base(TextureTarget.Texture2DArray, internalFormat, GenerateMipmapTarget.Texture2DArray, levels)
        {
            Initialize(width, height, layers);
        }

        private void Initialize(int width, int height, int layers)
        {
            Width = width;
            Height = height;
            Layers = layers;
            GL.BindTexture(TextureTarget.Texture2DArray, Handle);
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, Levels, InternalFormat, Width, Height, Layers);
            CheckError();
            SetDefaultTexParameters();
        }

        /// <summary>
        /// Uploads the given bitmap to the given layer of the 2D texture array.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="layer"></param>
        public void LoadBitmap(Bitmap bitmap, int layer)
        {
            if (bitmap.Width != Width || bitmap.Height != Height || FormatMapping.Get(bitmap).InternalFormat != InternalFormat)
                throw new ArgumentException("Bitmap incompatible to texture storage.");
            // flip bitmap
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // get the raw data and pass it to opengl
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                var map = FormatMapping.Get(bitmap);
                GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, layer, data.Width, data.Height, 1, map.PixelFormat, map.PixelType, data.Scan0);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            GL.Finish();
            CheckError();
        }
    }
}