using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public abstract class Vao
        : IReleasable
    {
        public int Handle
        {
            get { return VaoHandle; }
        }

        protected int VaoHandle;

        protected readonly BeginMode Mode;
        protected readonly int DrawCount;
        
        protected Vao(BeginMode mode, int drawCount)
        {
            Mode = mode;
            DrawCount = drawCount;
            // generate vertex array object
            GL.GenVertexArrays(1, out VaoHandle);
        }

        public virtual void Release()
        {
            GL.DeleteVertexArrays(1, ref VaoHandle);
        }

        public abstract void Render();
        
        public abstract void RenderInstanced(int instances);
    }
}