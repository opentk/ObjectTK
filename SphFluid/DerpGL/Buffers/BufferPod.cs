using System;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Buffers
{
    //TODO: refactor to be derived from Buffer<T> and containing an additional Buffer<T> instead of containing two Buffer<T>?
    public class BufferPod<T>
        : IDisposable
        where T : struct
    {
        public Buffer<T> Ping { private set; get; }
        public Buffer<T> Pong { private set; get; }

        public BufferPod()
        {
            Ping = new Buffer<T>();
            Pong = new Buffer<T>();
        }

        public void Dispose()
        {
            Ping.Dispose();
            Pong.Dispose();
        }

        public void Init(BufferTarget target, int elementCount, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Ping.Init(target, elementCount, usageHint);
            Pong.Init(target, elementCount, usageHint);
        }

        public void Init(BufferTarget target, T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw)
        {
            Ping.Init(target, data, usageHint);
            Pong.Init(target, data.Length, usageHint);
        }

        /// <summary>
        /// Changes the size of the buffer objects.
        /// </summary>
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