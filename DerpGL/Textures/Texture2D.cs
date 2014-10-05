using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DerpGL.Textures
{
    /// <summary>
    /// Represents a 2D texture.<br/>
    /// Images in this texture all are 2-dimensional. They have width and height, but no depth.
    /// </summary>
    public sealed class Texture2D
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
        /// Allocates immutable texture storage compatible to the given bitmap and fills it with its contents.<br/>
        /// Remember to call GenerateMipMaps() if levels is greater than 1 and you are using a mipmap filtering.
        /// </summary>
        /// <param name="bitmap">The bitmap to upload.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture2D(Bitmap bitmap, int levels)
            : this(FormatMapping.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height, levels)
        {
            LoadBitmap(bitmap);
        }

        /// <summary>
        /// Allocates immutable texture storage with the given parameters.
        /// </summary>
        /// <param name="internalFormat">The internal format to allocate.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="levels">The number of mipmap levels.</param>
        public Texture2D(SizedInternalFormat internalFormat, int width, int height, int levels = 1)
            : base(TextureTarget.Texture2D, internalFormat, levels)
        {
            Width = width;
            Height = height;
            GL.BindTexture(TextureTarget, Handle);
            GL.TexStorage2D((TextureTarget2d)TextureTarget, Levels, InternalFormat, Width, Height);
            CheckError();
        }

        /// <summary>
        /// Internal constructor used by <see cref="TextureFactory"/> to wrap a Texture2D instance around an already existing texture.
        /// </summary>
        internal Texture2D(int textureHandle, SizedInternalFormat internalFormat, int width, int height, int levels)
            : base(textureHandle, TextureTarget.Texture2D, internalFormat, levels)
        {
            Width = width;
            Height = height;
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
                GL.TexSubImage2D(TextureTarget, 0, 0, 0, data.Width, data.Height, map.PixelFormat, map.PixelType, data.Scan0);
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
            GL.BindTexture(TextureTarget, Handle);
            GL.GetTexImage(TextureTarget, 0, pixelFormat, pixelType, data);
            return data;
        }
    }
}