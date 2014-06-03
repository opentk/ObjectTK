using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    [DebuggerVisualizer(typeof(VboVisualizer), typeof(VboObjectSource), Description = "Vbo Visualizer")]
    public class Vbo<T>
        : ContextResource
        , IVbo
        where T : struct
    {
        /// <summary>
        /// A value indicating whether the buffer has been initialized and thus has access to allocated memory.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// The OpenGL handle to the buffer object.
        /// </summary>
        public int Handle { get; private set; }

        /// <summary>
        /// The OpenGL handle to the texture;
        /// </summary>
        public int TextureHandle { get; private set; }

        /// <summary>
        /// The size in bytes of one element within the buffer.
        /// </summary>
        public int ElementSize { get { return Marshal.SizeOf(typeof(T)); } }
        
        /// <summary>
        /// The number of elements for which buffer memory was allocated.
        /// </summary>
        public int ElementCount { get; private set; }

        /// <summary>
        /// The index to the element which will be written to on the next usage of SubData().
        /// </summary>
        public int CurrentElementIndex { get; set; }

        /// <summary>
        /// The number of elements with data explicitly written.
        /// </summary>
        public int ActiveElementCount { get; set; }

        /// <summary>
        /// The format to be used when accessing this buffer via the buffer texture.
        /// </summary>
        public SizedInternalFormat BufferTextureFormat
        {
            get
            {
                return _bufferTextureFormat;
            }
            set
            {
                _bufferTextureFormat = value;
                BindBufferToTexture();
            }
        }

        private SizedInternalFormat _bufferTextureFormat;

#if DEBUG
        /// <summary>
        /// Retrieves data back from vram for debugging purposes.
        /// </summary>
        public T[] Content
        {
            get
            {
                var items = new T[ElementCount];
                GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
                GL.GetBufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr)(ElementSize * ElementCount), items);
                return items;
            }
        }
#endif

        public void SaveToStream(Stream stream)
        {
            var writer = new BinaryWriter(stream);
            var items = new T[ElementCount];
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.GetBufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr) (ElementSize*ElementCount), items);
            writer.Write(ElementCount);
            foreach (var item in items)
            {
                writer.Write(item.ToString());
            }
            writer.Flush();
        }

        public Vbo()
        {
            Handle = GL.GenBuffer();
            TextureHandle = GL.GenTexture();
            // default internal buffer texture format
            _bufferTextureFormat = SizedInternalFormat.R32f;
            Initialized = false;
        }

        protected override void OnRelease()
        {
            GL.DeleteBuffer(Handle);
            GL.DeleteTexture(TextureHandle);
        }

        /// <summary>
        /// Allocates buffer memory and uploads given data to it.
        /// </summary>
        public void Init(BufferTarget bufferTarget, T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Init(bufferTarget, data.Length, data, usageHint);
            ActiveElementCount = data.Length;
            CurrentElementIndex = 0;
        }

        /// <summary>
        /// Allocates buffer memory and initializes it to zero.
        /// </summary>
        public void Init(BufferTarget bufferTarget, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Init(bufferTarget, elementCount, null, usageHint);
            ActiveElementCount = 0;
            CurrentElementIndex = 0;
        }

        protected void Init(BufferTarget bufferTarget, int elementCount, T[] data, BufferUsageHint usageHint)
        {
            Initialized = true;
            ElementCount = elementCount;
            var fullSize = elementCount * ElementSize;
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, data, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
            BindBufferToTexture();
        }

        /// <summary>
        /// Overwrites part of the buffer with the given data and automatically indexes forward through the available memory.
        /// Skips back to the beginning automatically once the end was reached.
        /// </summary>
        public void SubData(BufferTarget bufferTarget, T[] data)
        {
            if (data.Length > ElementCount) throw new ApplicationException(
                string.Format("Buffer not large enough to hold data. Buffer size: {0}. Elements to write: {1}.", ElementCount, data.Length));
            // check if data does not fit at the end of the buffer
            var rest = ElementCount - CurrentElementIndex;
            //TODO: can be simplified..
            if (rest >= data.Length)
            {
                // add the elements of data to the buffer at the current index
                SubData(bufferTarget, data, CurrentElementIndex);
                // skip forward through the buffer
                CurrentElementIndex += data.Length;
                // remember the total number of elements with data
                if (ActiveElementCount < CurrentElementIndex) ActiveElementCount = CurrentElementIndex;
                // skip back if the end was reached
                if (CurrentElementIndex >= ElementCount) CurrentElementIndex = 0;
            }
            else
            {
                // first fill the end of the buffer
                SubData(bufferTarget, data, CurrentElementIndex, rest);
                // proceed to add the remaining elements at the beginning
                rest = data.Length - rest;
                SubData(bufferTarget, data, 0, rest);
                CurrentElementIndex = rest;
                // remember that the full buffer was already written to
                ActiveElementCount = ElementCount;
            }
        }

        /// <summary>
        /// Overwrites part of the buffer with the given data at the given offset.
        /// Writes all data available in data.
        /// </summary>
        /// <param name="bufferTarget">The BufferTarget to use when binding the buffer.</param>
        /// <param name="data">The data to be transfered into the buffer.</param>
        /// <param name="offset">The index to the first element of the buffer to be overwritten.</param>
        public void SubData(BufferTarget bufferTarget, T[] data, int offset)
        {
            if (data.Length > ElementCount - offset) throw new ApplicationException(
                string.Format("Buffer not large enough to hold data. Buffer size: {0}. Offset: {1}. Elements to write: {2}.", ElementCount, offset, data.Length));
            SubData(bufferTarget, data, offset, data.Length);
        }

        /// <summary>
        /// Overwrites part of the buffer with the given data at the given offset.
        /// Writes <paramref name="count">count</paramref> elements of data.
        /// </summary>
        /// <param name="bufferTarget">The BufferTarget to use when binding the buffer.</param>
        /// <param name="data">The data to be transfered into the buffer.</param>
        /// <param name="offset">The index to the first element of the buffer to be overwritten.</param>
        /// <param name="count">The number of element from data to write.</param>
        public void SubData(BufferTarget bufferTarget, T[] data, int offset, int count)
        {
            if (count > ElementCount - offset) throw new ApplicationException(
                string.Format("Buffer not large enough to hold data. Buffer size: {0}. Offset: {1}. Elements to write: {2}.", ElementCount, offset, data.Length));
            if (count > data.Length) throw new ApplicationException(
                string.Format("Not enough data to write to buffer. Data length: {0}. Elements to write: {1}.", data.Length, count));
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferSubData(bufferTarget, (IntPtr)(ElementSize * offset), (IntPtr)(ElementSize * count), data);
        }

        protected void BindBufferToTexture()
        {
            if (!Initialized) return;
            GL.BindTexture(TextureTarget.TextureBuffer, TextureHandle);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, BufferTextureFormat, Handle);
        }

        protected void CheckBufferSize(BufferTarget bufferTarget, int size)
        {
            // check if uploaded size is correct
            int uploadedSize;
            GL.GetBufferParameter(bufferTarget, BufferParameterName.BufferSize, out uploadedSize);
            if (uploadedSize != size)
            {
                throw new ApplicationException(string.Format(
                    "Problem uploading data to buffer object. Tried to upload {0} bytes, but uploaded {1}.", size, uploadedSize));
            }
        }
    }

    public interface IVbo
    {
        void SaveToStream(Stream stream);
    }

    public class VboObjectSource
        : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            ((IVbo)target).SaveToStream(outgoingData);
        }
    }

    public class VboVisualizer
        : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var form = new Form
            {
                Text = "Vbo halt dein Maul",
                ClientSize = new Size(800, 600)
            };

            var box = new RichTextBox();

            var reader = new BinaryReader(objectProvider.GetData());
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                box.AppendText(reader.ReadString() + "\n");
            }

            box.Parent = form;
            box.Dock = DockStyle.Fill;
            form.Controls.Add(box);
            windowService.ShowDialog(form);
        }
    }
}