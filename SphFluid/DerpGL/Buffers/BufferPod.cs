using OpenTK.Graphics.OpenGL;

namespace DerpGL.Core.Buffers
{
    //TODO: refactor to be derived from Buffer<T> and containing an additional Buffer<T> instead of containing two Buffer<T>?
    public class BufferPod<T>
        : ContextResource
        where T : struct
    {
        public Buffer<T> Ping { private set; get; }
        public Buffer<T> Pong { private set; get; }

        public BufferPod()
        {
            Ping = new Buffer<T>();
            Pong = new Buffer<T>();
        }

        protected override void OnRelease()
        {
            Ping.Release();
            Pong.Release();
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