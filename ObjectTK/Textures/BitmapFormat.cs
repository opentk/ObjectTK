//
// BitmapFormat.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Textures
{
    internal class BitmapFormat
    {
        public SizedInternalFormat InternalFormat;
        public PixelFormat PixelFormat;
        public PixelType PixelType;

        private static readonly Dictionary<System.Drawing.Imaging.PixelFormat, BitmapFormat> FormatMap = new Dictionary
            <System.Drawing.Imaging.PixelFormat, BitmapFormat>
        {
            {
                System.Drawing.Imaging.PixelFormat.Format24bppRgb, new BitmapFormat
                {
                    InternalFormat = SizedInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Bgr,
                    PixelType = PixelType.UnsignedByte
                }
            },
            {
                System.Drawing.Imaging.PixelFormat.Format32bppArgb, new BitmapFormat
                {
                    InternalFormat = SizedInternalFormat.Rgba8,
                    PixelFormat = PixelFormat.Bgra,
                    PixelType = PixelType.UnsignedByte
                }
            }
        };

        // prevent instantiation
        protected BitmapFormat() { }

        static BitmapFormat()
        {
            // does not work
            //_formatMap.Add(System.Drawing.Imaging.PixelFormat.Format16bppRgb555, _formatMap[System.Drawing.Imaging.PixelFormat.Format16bppArgb1555]);
            // has alpha too? wtf?
            FormatMap.Add(System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                FormatMap[System.Drawing.Imaging.PixelFormat.Format32bppArgb]);
            FormatMap.Add(System.Drawing.Imaging.PixelFormat.Canonical,
                FormatMap[System.Drawing.Imaging.PixelFormat.Format32bppArgb]);
        }

        public static BitmapFormat Get(Bitmap bitmap)
        {
            if (FormatMap.ContainsKey(bitmap.PixelFormat)) return FormatMap[bitmap.PixelFormat];
            throw new ArgumentException("Error: Unsupported Pixel Format " + bitmap.PixelFormat);
        }
    }
}