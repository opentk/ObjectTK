using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class VboPod
        : IReleasable
    {
        public Vbo Ping { private set; get; }
        public Vbo Pong { private set; get; }

        public VboPod()
        {
            Ping = new Vbo();
            Pong = new Vbo();
        }

        public void Init(int elementCount, int elementSize)
        {
            Ping.AllocateData(BufferTarget.ArrayBuffer, elementCount, elementSize, BufferUsageHint.StreamDraw);
            Pong.AllocateData(BufferTarget.ArrayBuffer, elementCount, elementSize, BufferUsageHint.StreamDraw);
        }

        public void Init<T>(T[] data, int elementSize)
            where T : struct
        {
            Ping.UploadData(BufferTarget.ArrayBuffer, data, elementSize, BufferUsageHint.StreamDraw);
            Pong.AllocateData(BufferTarget.ArrayBuffer, data.Length, elementSize, BufferUsageHint.StreamDraw);
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