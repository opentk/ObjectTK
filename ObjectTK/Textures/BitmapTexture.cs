//
// BitmapTexture.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace ObjectTK.Textures
{
    /// <summary>
    /// Contains extension methods for texture types.
    /// </summary>
    public static class BitmapTexture
    {
        private static BitmapData LockBits(Bitmap bitmap)
        {
            return bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        }

        private static void CheckError()
        {
            GL.Finish();
            Utility.Assert("Error while uploading data to texture.");
        }

        /// <summary>
        /// Creates a new Texture2D instance compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">Specifies the bitmap to which the new texture will be compatible.</param>
        /// <param name="texture">Outputs the newly created texture.</param>
        /// <param name="levels">Specifies the number of mipmap levels.</param>
        public static void CreateCompatible(Bitmap bitmap, out Texture2D texture, int levels = 0)
        {
            texture = new Texture2D(BitmapFormat.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height, levels);
        }

        /// <summary>
        /// Creates a new Texture2DArray instance compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">Specifies the bitmap to which the new texture will be compatible.</param>
        /// <param name="texture">Outputs the newly created texture.</param>
        /// <param name="layers">Specifies the number of array layers the texture will contain.</param>
        /// <param name="levels">Specifies the number of mipmap levels.</param>
        public static void CreateCompatible(Bitmap bitmap, out Texture2DArray texture, int layers, int levels = 0)
        {
            texture = new Texture2DArray(BitmapFormat.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height, layers, levels);
        }

        /// <summary>
        /// Creates a new TextureCubemap instance with faces compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">Specifies the bitmap to which the new texture will be compatible.</param>
        /// <param name="texture">Outputs the newly created texture.</param>
        /// <param name="levels">Specifies the number of mipmap levels.</param>
        public static void CreateCompatible(Bitmap bitmap, out TextureCubemap texture, int levels = 0)
        {
            if (bitmap.Width != bitmap.Height) throw new ArgumentException("The faces of cube map textures must be square.");
            texture = new TextureCubemap(BitmapFormat.Get(bitmap).InternalFormat, bitmap.Width, levels);
        }

        /// <summary>
        /// Creates a new TextureCubemapArray instance with faces compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">Specifies the bitmap to which the new texture will be compatible.</param>
        /// <param name="layers">Specifies the number of array layers the texture will contain.</param>
        /// <param name="texture">Outputs the newly created texture.</param>
        /// <param name="levels">Specifies the number of mipmap levels.</param>
        public static void CreateCompatible(Bitmap bitmap, out TextureCubemapArray texture, int layers, int levels = 0)
        {
            if (bitmap.Width != bitmap.Height) throw new ArgumentException("The faces of cube map textures must be square.");
            texture = new TextureCubemapArray(BitmapFormat.Get(bitmap).InternalFormat, bitmap.Width, layers, levels);
        }

        /// <summary>
        /// Creates a new TextureRectangle instance compatible to the given bitmap.
        /// </summary>
        /// <param name="bitmap">Specifies the bitmap to which the new texture will be compatible.</param>
        /// <param name="texture">Outputs the newly created texture.</param>
        public static void CreateCompatible(Bitmap bitmap, out TextureRectangle texture)
        {
            texture = new TextureRectangle(BitmapFormat.Get(bitmap).InternalFormat, bitmap.Width, bitmap.Height);
        }

        /// <summary>
        /// Uploads the contents of a bitmap to the given texture level.<br/>
        /// Will result in an OpenGL error if the given bitmap is incompatible with the textures storage.
        /// </summary>
        public static void LoadBitmap(this Texture2D texture, Bitmap bitmap, int level = 0)
        {
            texture.Bind();
            var data = LockBits(bitmap);
            try
            {
                var map = BitmapFormat.Get(bitmap);
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
            var data = LockBits(bitmap);
            try
            {
                var map = BitmapFormat.Get(bitmap);
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
            var data = LockBits(bitmap);
            try
            {
                var map = BitmapFormat.Get(bitmap);
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