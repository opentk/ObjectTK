using System.Drawing;
using OpenTK.Graphics.OpenGL;
using SphFluid.Core.Buffers;

namespace SphFluid.Core.Shapes
{
    public abstract class ColoredShape
        : IndexedShape
    {
        public int[] Colors { get; protected set; }

        public static int ColorToRgba32(Color c)
        {
            return (c.A << 24) | (c.B << 16) | (c.G << 8) | c.R;
        }

        public override Vao CreateVao(PrimitiveType mode)
        {
            return new ColoredShapeVao(this, mode);
        }
    }
}