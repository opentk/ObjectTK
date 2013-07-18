using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shaders
{
    public class VertexAttrib
    {
        public int Index { get; private set; }
        public int Components { get; private set; }
        public VertexAttribPointerType Type { get; private set; }

        public VertexAttrib(int index, int components, VertexAttribPointerType type)
        {
            Index = index;
            Components = components;
            Type = type;
        }

        public void Bind(int stride, int offset, bool normalized = false)
        {
            // make sure the vertex attribute is enabled
            GL.EnableVertexAttribArray(Index);
            // set the vertex attribute pointer
            GL.VertexAttribPointer(Index, Components, Type, normalized, stride, offset);
        }
    }
}