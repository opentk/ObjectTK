using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace DerpGL.Shapes
{
    public class ColorCube
        : ColoredShape
    {
        public ColorCube()
        {
            DefaultMode = PrimitiveType.Triangles;

            // use default cube
            var cube = new Cube();
            Vertices = cube.Vertices;
            Indices = cube.Indices;

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