using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using OpenTK;

namespace DerpGL
{
    public static class Extensions
    {
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.R/255f, color.G/255f, color.B/255f, color.A/255f);
        }

        public static uint ToRgba32(this Color color)
        {
            return (uint)(color.A << 24 | color.B << 16 | color.G << 8 | color.R);
        }

        public static Color Rgba32ToColor(this uint color)
        {
            const uint mask = 0x000000FF;
            return Color.FromArgb((int)(color >> 24 & mask), (int)(color & mask), (int)(color >> 8 & mask), (int)(color >> 16 & mask));
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo type, bool inherit)
        {
            return type.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }
    }
}
