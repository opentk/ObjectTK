#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
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