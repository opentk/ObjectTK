namespace SphFluid.Core.Buffers
{
    public interface ITexture
    {
        bool IsInitialized { get; }
        void Initialize(int width, int height, int depth);
    }
}