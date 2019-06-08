//
// BufferPod.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Buffers
{
    /// <summary>
    /// Represents two buffer objects which are bundled together to simplify ping-ponging.
    /// </summary>
    /// <typeparam name="T">The type of elements in the buffer objects.</typeparam>
    public class BufferPod<T>
        : GLResource
        where T : struct
    {
        /// <summary>
        /// First or front buffer.
        /// </summary>
        public Buffer<T> Ping { private set; get; }

        /// <summary>
        /// Second or back buffer.
        /// </summary>
        public Buffer<T> Pong { private set; get; }

        /// <summary>
        /// Requests two new, uninitialized <see cref="Buffer{T}"/> objects.
        /// </summary>
        public BufferPod()
        {
            Ping = new Buffer<T>();
            Pong = new Buffer<T>();
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            Ping.Dispose();
            Pong.Dispose();
        }

        /// <summary>
        /// Allocates memory for both buffer objects without initializing it.
        /// </summary>
        /// <param name="target">The BufferTarget to use when binding the buffers.</param>
        /// <param name="elementCount">The number of elements for each buffer to allocate memory for.</param>
        /// <param name="usageHint">The usage hint for both buffer objects.</param>
        public void Init(BufferTarget target, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Ping.Init(target, elementCount, usageHint);
            Pong.Init(target, elementCount, usageHint);
        }

        /// <summary>
        /// Allocates memory for both buffer objects and initializes the first buffer with the given data.
        /// The second buffer is left uninitialized.
        /// </summary>
        /// <param name="target">The BufferTarget to use when binding the buffers.</param>
        /// <param name="data">The data to upload to the first buffer and allocate enough memory for in the second buffer.</param>
        /// <param name="usageHint">The usage hint for both buffer objects.</param>
        public void Init(BufferTarget target, T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Ping.Init(target, data, usageHint);
            Pong.Init(target, data.Length, usageHint);
        }

        /// <summary>
        /// Changes the size of both buffer objects.
        /// </summary>
        /// <param name="target">The BufferTarget to use when binding the buffers.</param>
        /// <param name="elementCount">The new number of elements for each buffer to allocate memory for.</param>
        /// <param name="usageHint">The usage hint for both buffer objects.</param>
        public void Resize(BufferTarget target, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            // wrap current element index into the new buffer size if it is smaller than before
            Ping.CurrentElementIndex %= elementCount;
            // prevent active element count from exceeding the new buffer size
            Ping.ActiveElementCount = Math.Min(Ping.ActiveElementCount, elementCount);
            // copy data into resized buffer
            Pong.Init(target, elementCount, usageHint);
            if (Ping.Initialized) Pong.CopyFrom(Ping);
            // swap buffers
            Swap();
            // resize the other buffer
            Pong.Init(target, elementCount, usageHint);
        }

        /// <summary>
        /// Swaps the two buffer objects.
        /// </summary>
        public void Swap()
        {
            // copy over current "state"
            Pong.CurrentElementIndex = Ping.CurrentElementIndex;
            Pong.ActiveElementCount = Ping.ActiveElementCount;
            // swap buffers
            var tmp = Ping;
            Ping = Pong;
            Pong = tmp;
        }
    }
}