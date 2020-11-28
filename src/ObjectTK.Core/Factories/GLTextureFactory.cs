using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using JetBrains.Annotations;
using ObjectTK.GLObjects;
using OpenTK.Graphics.OpenGL;

// ReSharper disable once CheckNamespace
namespace ObjectTK {
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class GLTextureFactory {
        public static GLTextureFactory Instance { get; } = new GLTextureFactory();
        private GLTextureFactory() { }
        
        /// Creates a texture from a raw pointer.
        /// This is typically used for creation from a bitmap.
        [NotNull]
        [MustUseReturnValue]
        public Texture2D Create2D(string name, [NotNull] TextureConfig cfg, int width, int height, IntPtr data) {
            var t = GL.GenTexture();
            var label = $"Texture2D: {name}";
            GL.BindTexture(TextureTarget.Texture2D, t);
            GL.TexImage2D(TextureTarget.Texture2D, 0, cfg.InternalFormat, width, height, 0, cfg.PixelFormat, cfg.PixelType, data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) cfg.MagFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) cfg.MinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) cfg.WrapS);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) cfg.WrapS);
            GL.ObjectLabel(ObjectLabelIdentifier.Texture, t, label.Length, label);
            if (cfg.GenerateMipmaps) {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
            
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return new Texture2D(t, name, cfg.InternalFormat, width, height);
        }
        
        /// Creates a 2D texture from a bitmap.
        [NotNull]
        [MustUseReturnValue]
        public Texture2D FromBitmap([NotNull] string name, [NotNull] TextureConfig cfg, [NotNull] Bitmap bmp) {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            return Create2D(name, cfg, bmp.Width, bmp.Height, bitmapData.Scan0);
        }


        // Hide the default members of this object for a cleaner API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            // ReSharper disable once BaseObjectEqualsIsObjectEquals
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        // ReSharper disable once AnnotateCanBeNullTypeMember
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [NotNull]
        public new Type GetType() {
            return base.GetType();
        }
    }
}
