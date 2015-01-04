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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Tools.Shapes
{
    public class Cube
        : IndexedShape
    {
        public Cube()
        {
            DefaultMode = PrimitiveType.Triangles;

            Vertices = new[]
            {
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f)
            };
            
            Indices = new uint[]
            {
                // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                5, 1, 0, 0, 4, 5,
                // right face
                1, 5, 6, 6, 2, 1
            };
        }
    }
}
