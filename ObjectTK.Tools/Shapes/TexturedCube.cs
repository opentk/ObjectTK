//
// TexturedCube.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace ObjectTK.Tools.Shapes
{
    public class TexturedCube
        : TexturedShape
    {
        public TexturedCube()
        {
            DefaultMode = PrimitiveType.Triangles;

            var quad_uv_map = new[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            };

            // use default cube
            using (var cube = new Cube())
            {
                // Cube uses indexed vertices, TexturedShape assumes a flat vertices array
                // So we need to assemble the missing vertices ourself
                Vertices = cube.Indices.Select(idx => cube.Vertices[idx]).ToArray();

                // Use predefined uv texture mapping for vertices
                TexCoords = Enumerable.Range(0, Vertices.Length).Select(i => quad_uv_map[i % quad_uv_map.Length]).ToArray();
            }
        }
    }
}
