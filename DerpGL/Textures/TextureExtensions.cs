using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DerpGL.Textures
{
    /// <summary>
    /// Contains extension methods for texture types.
    /// </summary>
    public static class TextureExtensions
    {
        private static BitmapData GetData(Bitmap bitmap)
        {
            // flip bitmap
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // get the raw data and pass it to opengl
            return bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        }

        private static void CheckError()
        {
            GL.Finish();
            Utility.Assert("Error while uploading data to texture.");
        }

        /// <summary>
        /// Uploads the contents of a bitmap to the given texture level.<br/>
        /// Will result in an OpenGL error if the given bitmap is incompatible with the textures storage.
        /// </summary>
        public static void LoadBitmap(this Texture2D texture, Bitmap bitmap, int level = 0)
        {
            texture.Bind();
            var data = GetData(bitmap);
            try
            {
                var map = FormatMapping.Get(bitmap);
                GL.TexSubImage2D(texture.TextureTarget, level, 0, 0, data.Width, data.Height,
                    map.PixelFormat, map.PixelType, data.Scan0);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            CheckError();
        }

        /// <summary>
        /// Uploads the contents of a bitmap to the given texture layer and level.<br/>
        /// Will result in an OpenGL error if the given bitmap is incompatible with the textures storage.
        /// </summary>
        public static void LoadBitmap(this LayeredTexture texture, Bitmap bitmap, int layer, int level = 0)
        {
            texture.Bind();
            var data = GetData(bitmap);
            try
            {
                var map = FormatMapping.Get(bitmap);
                GL.TexSubImage3D(texture.TextureTarget, level, 0, 0, layer, data.Width, data.Height, 1,
                    map.PixelFormat, map.PixelType, data.Scan0);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            CheckError();
        }

        /// <summary>
        /// Uploads the contents of the bitmaps to all faces of the given cube map level.<br/>
        /// Will result in an OpenGL error if any of the given bitmaps is incompatible with the textures storage.
        /// </summary>
        public static void LoadBitmap(this TextureCubemap texture, Bitmap[] bitmaps, int level = 0)
        {
            if (bitmaps.Length != 6) throw new ArgumentException("Expected exactly 6 bitmaps for a cube map.");
            // load all six faces
            for (var face = 0; face < 6; face++) texture.LoadBitmap(bitmaps[face], face, level);
        }

        /// <summary>
        /// Uploads the contents of a bitmap to a single face of the given cube map level.<br/>
        /// Will result in an OpenGL error if the given bitmap is incompatible with the textures storage.
        /// </summary>
        public static void LoadBitmap(this TextureCubemap texture, Bitmap bitmap, int face, int level = 0)
        {
            const TextureTarget firstFace = TextureTarget.TextureCubeMapPositiveX;
            texture.Bind();
            var data = GetData(bitmap);
            try
            {
                var map = FormatMapping.Get(bitmap);
                GL.TexSubImage2D(firstFace + face, level, 0, 0, data.Width, data.Height,
                    map.PixelFormat, map.PixelType, data.Scan0);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            CheckError();
        }

        /// <summary>
        /// Retrieves the texture data.
        /// </summary>
        public static T[,] GetContent<T>(this Texture2D texture, PixelFormat pixelFormat, PixelType pixelType, int level = 0)
            where T : struct
        {
            var data = new T[texture.Width, texture.Height];
            texture.Bind();
            GL.GetTexImage(texture.TextureTarget, level, pixelFormat, pixelType, data);
            return data;
        }
    }
}