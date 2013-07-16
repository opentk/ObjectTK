using System.Drawing;
using OpenTK;

namespace SphFluid
{
    public static class Extensions
    {
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.R/255f, color.G/255f, color.B/255f, color.A/255f);
        }
    }
}
