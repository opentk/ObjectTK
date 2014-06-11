using System.Drawing;
using OpenTK.Graphics.OpenGL;

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
            Colors = new[]
            {
                Color.DarkRed.ToRgba32(),
                Color.DarkRed.ToRgba32(),
                Color.Gold.ToRgba32(),
                Color.Gold.ToRgba32(),
                Color.DarkRed.ToRgba32(),
                Color.DarkRed.ToRgba32(),
                Color.Gold.ToRgba32(),
                Color.Gold.ToRgba32()
            };
        }
    }
}