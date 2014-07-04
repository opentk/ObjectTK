using System;
using DerpGL.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public abstract class Shape
        : IDisposable
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }
        public Buffer<Vector3> VertexBuffer { get; protected set; }

        public virtual void UpdateBuffers()
        {
            VertexBuffer = new Buffer<Vector3>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
        }

        public virtual void Dispose()
        {
            if (VertexBuffer != null) VertexBuffer.Dispose();
        }

        public void RenderImmediate()
        {
            RenderImmediate(DefaultMode);
        }

        public virtual void RenderImmediate(PrimitiveType mode)
        {
            GL.Begin(mode);
            foreach (var vertex in Vertices)
            {
                GL.Vertex3(vertex);
            }
            GL.End();
        }
    }
}
