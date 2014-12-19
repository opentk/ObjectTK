#region License
// ObjectTK License
// Copyright (C) 2013-2014 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using OpenTK;

namespace ObjectTK
{
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Transforms this <see cref="Color"/> object to a <see cref="Vector4"/>.<br/>
        /// The resulting vector contains values in the range of (0,1).
        /// </summary>
        /// <param name="color">The Color object to transform.</param>
        /// <returns>A Vector4 object containing the color information.</returns>
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.R/255f, color.G/255f, color.B/255f, color.A/255f);
        }

        /// <summary>
        /// Transforms this <see cref="Color"/> object to an unsigned integer.<br/>
        /// The components are formatted compatible to OpenGL.
        /// </summary>
        /// <param name="color">The Color object to transform.</param>
        /// <returns>An unsigned integer containing the color information.</returns>
        public static uint ToRgba32(this Color color)
        {
            return (uint)(color.A << 24 | color.B << 16 | color.G << 8 | color.R);
        }

        /// <summary>
        /// Transforms this unsigned integer to a <see cref="Color"/> object.<br/>
        /// Requires the information formatted like the output of <see cref="ToRgba32"/>.
        /// </summary>
        /// <param name="color">The unsigned integer to transform.</param>
        /// <returns>A Color object containing the same information.</returns>
        public static Color Rgba32ToColor(this uint color)
        {
            const uint mask = 0x000000FF;
            return Color.FromArgb((int)(color >> 24 & mask), (int)(color & mask), (int)(color >> 8 & mask), (int)(color >> 16 & mask));
        }

        /// <summary>
        /// Retrieves custom attributes in a typed enumerable.
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for. Only attributes that are assignable to this type are returned.</typeparam>
        /// <param name="type">The member on which to look for custom attributes.</param>
        /// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
        /// <returns>An IEnumerable of custom attributes applied to this member.</returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo type, bool inherit)
        {
            return type.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }
    }
}
