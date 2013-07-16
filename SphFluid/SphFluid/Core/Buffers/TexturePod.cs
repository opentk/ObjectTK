namespace SphFluid.Core.Buffers
{
    public class TexturePod
        : ITexture
        , IReleasable
    {
        public bool IsInitialized
        {
            get { return Ping.IsInitialized && Pong.IsInitialized; }
        }

        public Texture Ping { get; private set; }
        public Texture Pong { get; private set; }

        public TexturePod(int numComponents)
        {
            Ping = new Texture(numComponents);
            Pong = new Texture(numComponents);
        }

        public void Initialize(int width, int height, int depth)
        {
            Ping.Initialize(width, height, depth);
            Pong.Initialize(width, height, depth);
        }

        public void Release()
        {
            Ping.Release();
            Pong.Release();
        }

        public void Swap()
        {
            var tmp = Ping;
            Ping = Pong;
            Pong = tmp;
        }
    }
}