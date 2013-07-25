using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shapes
{
    public class Circle
        : Shape
    {
        public Circle(float radius)
        {
            DefaultMode = BeginMode.TriangleFan;
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