using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Textures
{
    internal class FormatMapping
    {
        public SizedInternalFormat InternalFormat;
        public PixelFormat PixelFormat;
        public PixelType PixelType;

        private static readonly Dictionary<System.Drawing.Imaging.PixelFormat, FormatMapping> FormatMap = new Dictionary
            <System.Drawing.Imaging.PixelFormat, FormatMapping>
        {
            {
                System.Drawing.Imaging.PixelFormat.Format24bppRgb, new FormatMapping
                {
                    InternalFormat = SizedInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Bgr,
                    PixelType = PixelType.UnsignedByte
                }
            },
            {
                System.Drawing.Imaging.PixelFormat.Format32bppArgb, new FormatMapping
                {
                    InternalFormat = SizedInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Bgra,
                    PixelType = PixelType.UnsignedByte
                }
            }
        };

        // prevent instantiation
        protected FormatMapping() { }

        static FormatMapping()
        {
            // does not work
            //_formatMap.Add(System.Drawing.Imaging.PixelFormat.Format16bppRgb555, _formatMap[System.Drawing.Imaging.PixelFormat.Format16bppArgb1555]);
            // has alpha too? wtf?
            FormatMap.Add(System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                FormatMap[System.Drawing.Imaging.PixelFormat.Format32bppArgb]);
            FormatMap.Add(System.Drawing.Imaging.PixelFormat.Canonical,
                FormatMap[System.Drawing.Imaging.PixelFormat.Format32bppArgb]);
        }

        public static FormatMapping Get(Bitmap bitmap)
        {
            if (FormatMap.ContainsKey(bitmap.PixelFormat)) return FormatMap[bitmap.PixelFormat];
            throw new ArgumentException("Error: Unsupported Pixel Format " + bitmap.PixelFormat);
        }
    }
}