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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
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