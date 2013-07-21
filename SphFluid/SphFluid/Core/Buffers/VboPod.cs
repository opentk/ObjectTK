using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class VboPod<T>
        : IReleasable
        where T : struct
    {
        public Vbo<T> Ping { private set; get; }
        public Vbo<T> Pong { private set; get; }

        public VboPod()
        {
            Ping = new Vbo<T>();
            Pong = new Vbo<T>();
        }

        public void Init(BufferTarget target, int elementCount)
        {
            Ping.Init(target, elementCount, BufferUsageHint.StreamDraw);
            Pong.Init(target, elementCount, BufferUsageHint.StreamDraw);
        }

        public void Init(BufferTarget target, T[] data)
        {
            Ping.Init(target, data, BufferUsageHint.StreamDraw);
            Pong.Init(target, data.Length, BufferUsageHint.StreamDraw);
        }

        public void Swap()
        {
            var tmp = Ping;
            Ping = Pong;
            Pong = tmp;
        }

        public virtual void Release()
        {
            Ping.Release();
            Pong.Release();
        }
    }
}