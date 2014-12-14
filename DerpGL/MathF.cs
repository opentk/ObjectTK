#region License
// DerpGL License
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
using System;
using OpenTK;

namespace DerpGL
{
    /// <summary>
    /// Provides mathematical constants and functions with float precision.
    /// </summary>
    public static class MathF
    {
        /// <summary>
        /// Represents PI with float precision.
        /// </summary>
        public const float PI = (float) Math.PI;

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="angle">An angle, measured in radians.</param>
        /// <returns>The sine of angle. If angle is equal to NaN, NegativeInfinity, or PositiveInfinity, this method returns NaN.</returns>
        public static float Sin(float angle)
        {
            return (float)Math.Sin(angle);
        }

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="angle">An angle, measured in radians.</param>
        /// <returns>The cosine of angle. If angle is equal to NaN, NegativeInfinity, or PositiveInfinity, this method returns NaN.</returns>
        public static float Cos(float angle)
        {
            return (float)Math.Cos(angle);
        }

        /// <summary>
        /// Return the normal matrix, that is the upper 3x3 part of the inverted and transposed matrix.
        /// </summary>
        /// <param name="matrix">Specifies the transformation matrix.</param>
        /// <returns>The normal matrix.</returns>
        public static Matrix3 GetNormalMatrix(this Matrix4 matrix)
        {
            matrix.Invert();
            matrix.Transpose();
            return new Matrix3(matrix);
        }
    }
}