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
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public class Circle
        : Shape
    {
        public Circle(float radius)
        {
            DefaultMode = PrimitiveType.TriangleFan;
            const int z = 0;
            const int slices = 64;
            const float dtheta = MathHelper.TwoPi / (slices - 1);
            var theta = 0f;
            Vertices = new Vector3[slices+1];
            Vertices[0] = new Vector3(0, 0, z);
            for (var i = 0; i < slices; i++)
            {
                Vertices[i+1] = new Vector3((float)Math.Cos(theta) * radius, (float)Math.Sin(theta) * radius, z);
                theta += dtheta;
            }
        }
    }
}