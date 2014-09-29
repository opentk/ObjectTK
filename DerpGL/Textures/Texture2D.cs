using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D texture.
    /// </summary>
    public class Texture2D
        : Texture
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
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        public Texture2D(SizedInternalFormat internalFormat, int width, int height)
            : base(TextureTarget.Texture2D, internalFormat)
        {
            Width = width;
            Height = height;
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, internalFormat, width, height);
            CheckError();
        }

        /// <summary>
        /// Allocates immutable texture storage compatible to the given bitmap and fills it with its contents.
        /// </summary>
        /// <param name="bitmap"></param>
        public Texture2D(Bitmap bitmap)
            : this(FormatMapping.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height)
        {
            SetDefaultTexParameters();
            LoadBitmap(bitmap);
        }

        /// <summary>
        /// Uploads the contents of the given bitmap to the texture memory.
        /// </summary>
        /// <param name="bitmap">The bitmap to upload.</param>
        public void LoadBitmap(Bitmap bitmap)
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
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, data.Width, data.Height, map.PixelFormat, map.PixelType, data.Scan0);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            GL.Finish();
            CheckError();
        }

        /// <summary>
        /// Retrieves the texture data.
        /// </summary>
        public T[,] GetContent<T>(PixelFormat pixelFormat, PixelType pixelType)
            where T : struct
        {
            var data = new T[Width, Height];
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.GetTexImage(TextureTarget.Texture2D, 0, pixelFormat, pixelType, data);
            return data;
        }
    }
}