//
// Rect.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Tools.Shapes
{
    public class Rect
        : Shape
    {
        public Rect(float right, float top)
            : this(-1, -1, right, top)
        {
        }

        public Rect(float left, float bottom, float right, float top)
        {
            DefaultMode = PrimitiveType.LineLoop;
            const int z = 0;
            Vertices = new[]
            {
                new Vector3(left, bottom, z),
                new Vector3(right, bottom, z),
                new Vector3(right, top, z),
                new Vector3(left, top, z)
            };
        }
    }
}