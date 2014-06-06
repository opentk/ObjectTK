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
                ColorToRgba32(Color.DarkRed),
                ColorToRgba32(Color.DarkRed),
                ColorToRgba32(Color.Gold),
                ColorToRgba32(Color.Gold),
                ColorToRgba32(Color.DarkRed),
                ColorToRgba32(Color.DarkRed),
                ColorToRgba32(Color.Gold),
                ColorToRgba32(Color.Gold)
            };
        }
    }
}