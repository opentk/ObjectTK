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
    public class LayeredQuads
        : IndexedShape
    {
        public LayeredQuads(int layers)
        {
            DefaultMode = PrimitiveType.Triangles;
            const float size = 1;
            var z = -size / 2f;
            var step = size / (layers-1);
            Vertices = new Vector3[4*layers];
            Indices = new uint[6*layers];
            var order = new uint[]{ 0, 1, 2, 2, 3, 0 };
            for (uint i = 0; i < layers; ++i)
            {
                Vertices[i * 4 + 0] = new Vector3(-size / 2, -size / 2, z);
                Vertices[i * 4 + 1] = new Vector3(+size / 2, -size / 2, z);
                Vertices[i * 4 + 2] = new Vector3(+size / 2, +size / 2, z);
                Vertices[i * 4 + 3] = new Vector3(-size / 2, +size / 2, z);
                for (uint j = 0; j < 6; j++) Indices[i * 6 + j] = i*4 + order[j];
                z += step;
            }
        }
    }
}