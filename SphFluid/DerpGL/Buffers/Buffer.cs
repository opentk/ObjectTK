using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    /// <summary>
    /// Handles initialization and access to a collection of elements of type T in a buffer object.
    /// </summary>
    /// <typeparam name="T">The type of elements in the buffer object.</typeparam>
    public class Buffer<T>
        : ContextResource
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
        /// Retrieves data back from vram.
        /// Mainly for debugging purposes.
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

        /// <summary>
        /// Requests a new, uninitialized buffer object.
        /// </summary>
        public Buffer()
        {
            Handle = GL.GenBuffer();
            Initialized = false;
        }

        protected override void OnRelease()
        {
            GL.DeleteBuffer(Handle);
        }

        /// <summary>
        /// Allocates buffer memory and uploads given data to it.
        /// </summary>
        /// <param name="bufferTarget">The BufferTarget to use when binding the buffer.</param>
        /// <param name="data">The data to be transfered into the buffer.</param>
        /// <param name="usageHint">The usage hint of the buffer object.</param>
        public void Init(BufferTarget bufferTarget, T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Init(bufferTarget, data.Length, data, usageHint);
            ActiveElementCount = data.Length;
            CurrentElementIndex = 0;
        }

        /// <summary>
        /// Allocates buffer memory without initializing it.
        /// </summary>
        /// <param name="bufferTarget">The BufferTarget to use when binding the buffer.</param>
        /// <param name="elementCount">The number of elements to allocate memory for.</param>
        /// <param name="usageHint">The usage hint of the buffer object.</param>
        public void Init(BufferTarget bufferTarget, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Init(bufferTarget, elementCount, null, usageHint);
            ActiveElementCount = 0;
            CurrentElementIndex = 0;
        }

        protected virtual void Init(BufferTarget bufferTarget, int elementCount, T[] data, BufferUsageHint usageHint)
        {
            Initialized = true;
            ElementCount = elementCount;
            var fullSize = elementCount * ElementSize;
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferData(bufferTarget, (IntPtr)fullSize, data, usageHint);
            CheckBufferSize(bufferTarget, fullSize);
        }

        /// <summary>
        /// Overwrites part of the buffer with the given data and automatically indexes forward through the available memory.
        /// Skips back to the beginning automatically once the end was reached.
        /// </summary>
        /// <param name="bufferTarget">The BufferTarget to use when binding the buffer.</param>
        /// <param name="data">The data to be transfered into the buffer.</param>
        public void SubData(BufferTarget bufferTarget, T[] data)
        {
            if (data.Length > ElementCount) throw new ApplicationException(
                string.Format("Buffer not large enough to hold data. Buffer size: {0}. Elements to write: {1}.", ElementCount, data.Length));
            // check if data does not fit at the end of the buffer
            var rest = ElementCount - CurrentElementIndex;
            if (rest >= data.Length)
            {
                // add the elements of data to the buffer at the current index
                SubData(bufferTarget, data, CurrentElementIndex);
                // skip forward through the buffer
                CurrentElementIndex += data.Length;
                // remember the total number of elements with data
                if (ActiveElementCount < CurrentElementIndex) ActiveElementCount = CurrentElementIndex;
                // skip back if the end was reached
                // in this case it can only be reached exactly because otherwise it would be handled by the else-case
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
        /// Writes <paramref name="count" /> elements of data.
        /// </summary>
        /// <param name="bufferTarget">The BufferTarget to use when binding the buffer.</param>
        /// <param name="data">The data to be transfered into the buffer.</param>
        /// <param name="offset">The index to the first element of the buffer to be overwritten.</param>
        /// <param name="count">The number of elements from data to write.</param>
        public void SubData(BufferTarget bufferTarget, T[] data, int offset, int count)
        {
            if (count > ElementCount - offset) throw new ApplicationException(
                string.Format("Buffer not large enough to hold data. Buffer size: {0}. Offset: {1}. Elements to write: {2}.", ElementCount, offset, data.Length));
            if (count > data.Length) throw new ApplicationException(
                string.Format("Not enough data to write to buffer. Data length: {0}. Elements to write: {1}.", data.Length, count));
            GL.BindBuffer(bufferTarget, Handle);
            GL.BufferSubData(bufferTarget, (IntPtr)(ElementSize * offset), (IntPtr)(ElementSize * count), data);
        }

        /// <summary>
        /// Fills this buffer with data from another data.
        /// Copied on server-side only, no synchronization or transfer of data to host required.
        /// </summary>
        /// <param name="source">The source buffer to copy data from.</param>
        /// <param name="readOffset">Element offset into the source buffer.</param>
        /// <param name="writeOffset">Element offset into this buffer</param>
        /// <param name="count">The Number of elements to copy.</param>
        public void CopyFrom(Buffer<T> source, int readOffset, int writeOffset, int count)
        {
            GL.BindBuffer(BufferTarget.CopyReadBuffer, source.Handle);
            GL.BindBuffer(BufferTarget.CopyWriteBuffer, Handle);
            GL.CopyBufferSubData(BufferTarget.CopyReadBuffer, BufferTarget.CopyWriteBuffer,
                (IntPtr)(ElementSize * readOffset), (IntPtr)(ElementSize * writeOffset), (IntPtr)(ElementSize * count));
        }

        /// <summary>
        /// Checks if uploaded size matches the expected size.
        /// </summary>
        protected void CheckBufferSize(BufferTarget bufferTarget, int size)
        {
            int uploadedSize;
            GL.GetBufferParameter(bufferTarget, BufferParameterName.BufferSize, out uploadedSize);
            if (uploadedSize != size)
            {
                throw new ApplicationException(string.Format(
                    "Problem uploading data to buffer object. Tried to upload {0} bytes, but uploaded {1}.", size, uploadedSize));
            }
        }
    }
}