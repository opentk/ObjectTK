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