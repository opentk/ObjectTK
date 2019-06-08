//
// MathF.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK;

namespace ObjectTK
{
    /// <summary>
    /// Provides mathematical constants and functions with float precision.
    /// </summary>
    public static class MathF
    {
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