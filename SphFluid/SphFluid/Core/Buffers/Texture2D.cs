using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace SphFluid.Core.Buffers
{
    [DebuggerVisualizer(typeof(Texture2DVisualizer), typeof(TextureObjectSource), Description = "Texture2D Visualizer")]
    public class Texture2D
        : Texture
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public void Initialize(PixelInternalFormat internalFormat, int width, int height, PixelFormat pixelFormat, PixelType pixelType)
        {
            PixelInternalFormat = internalFormat;
            Width = width;
            Height = height;
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
            CheckError();
        }

        public void SaveImageToStream(Stream stream)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            var data = new int[Width*Height*3];
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data);
            var bitmap = new Bitmap(Width, Height);
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    var newColor = Color.FromArgb(data[i + j*Width + 0], data[i + j*Width + 1], data[i + j*Width + 2]);
                    bitmap.SetPixel(i, j, newColor);
                }
            }
            bitmap.Save(stream, ImageFormat.Bmp);
        }
    }

    public class TextureObjectSource
        : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            ((Texture2D) target).SaveImageToStream(outgoingData);
        }
    }

    public class Texture2DVisualizer
        : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var image = Image.FromStream(objectProvider.GetData());

            var form = new Form
            {
                Text = string.Format("Width: {0}, Height: {1}", image.Width, image.Height),
                ClientSize = new Size(image.Width, image.Height),
                FormBorderStyle = FormBorderStyle.FixedToolWindow
            };
            
            form.Controls.Add(new PictureBox
            {
                Image = image,
                Parent = form,
                Dock = DockStyle.Fill
            });

            windowService.ShowDialog(form);
        }
    }
}