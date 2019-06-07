//
// ColorCube.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Tools.Shapes
{
    public class ColorCube
        : ColoredShape
    {
        public ColorCube()
        {
            DefaultMode = PrimitiveType.Triangles;

            // use default cube
            using (var cube = new Cube())
            {
                Vertices = cube.Vertices;
                Indices = cube.Indices;
            }

            // add color to the vertices
            Colors = new List<Color>
            {
                Color.DarkRed,
                Color.DarkRed,
                Color.Gold,
                Color.Gold,
                Color.DarkRed,
                Color.DarkRed,
                Color.Gold,
                Color.Gold
            }.Select(_ => _.ToRgba32()).ToArray();
        }
    }
}