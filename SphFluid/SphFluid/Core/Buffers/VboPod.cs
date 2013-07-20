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

        public void Init(int elementCount)
        {
            Ping.AllocateData(BufferTarget.ArrayBuffer, elementCount, BufferUsageHint.StreamDraw);
            Pong.AllocateData(BufferTarget.ArrayBuffer, elementCount, BufferUsageHint.StreamDraw);
        }

        public void Init(T[] data)
        {
            Ping.UploadData(BufferTarget.ArrayBuffer, data, BufferUsageHint.StreamDraw);
            Pong.AllocateData(BufferTarget.ArrayBuffer, data.Length, BufferUsageHint.StreamDraw);
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