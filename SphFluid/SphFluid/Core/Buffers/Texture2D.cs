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
        public PixelFormat PixelFormat { get; private set; }
        public PixelType PixelType { get; private set; }
        
        public void Initialize(PixelInternalFormat format, int width, int height, PixelFormat pixelFormat, PixelType pixelType)
        {
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            PixelType = pixelType;
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, format, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
            CheckError();
        }

        public void SaveImageToStream(Stream stream)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            var data = new float[Width*Height];
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat, PixelType, data);
            var bitmap = new Bitmap(Width, Height);
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    var c = (int) (data[i+j*Width] / 32f * 255);
                    var newColor = Color.FromArgb(c,c,c);
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