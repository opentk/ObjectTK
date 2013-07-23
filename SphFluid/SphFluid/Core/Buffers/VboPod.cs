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