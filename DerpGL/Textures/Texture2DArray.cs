using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D texture array.<br/>
    /// Images in this texture all are 2-dimensional. However, it contains multiple sets of 2-dimensional images,
    /// all within one texture. The array length is part of the texture's size.
    /// </summary>
    public sealed class Texture2DArray
        : LayeredTexture
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture2DArray; } }

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
        }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="layers">The number of layers to allocate.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture2DArray(SizedInternalFormat internalFormat, int width, int height, int layers, int levels = 1)
            : base(internalFormat, levels)
        {
            Width = width;
            Height = height;
            Layers = layers;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage3D((TextureTarget3d)TextureTarget, Levels, InternalFormat, Width, Height, Layers);
            CheckError();
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
                GL.TexSubImage3D(TextureTarget, 0, 0, 0, layer, data.Width, data.Height, 1, map.PixelFormat, map.PixelType, data.Scan0);
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