//
// Shape.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Tools.Shapes
{
    /// <summary>
    /// TODO: needs a total refactoring
    /// does not prevent multiple calls to UpdateBuffers() and causes resource leaks
    /// does not fit well to an inheritance chain
    /// there may be a shape just with vertices
    /// there may be an indexed shape with vertices and indices
    /// there may be a colored, indexed shape with vertices, indices and colors
    /// there may be a colored shape with vertices and colors but no indices...
    /// </summary>
    public abstract class Shape
        : GLResource
    {
        public PrimitiveType DefaultMode { get; set; }
        public Vector3[] Vertices { get; protected set; }
        public Buffer<Vector3> VertexBuffer { get; protected set; }

        public virtual void UpdateBuffers()
        {
            VertexBuffer = new Buffer<Vector3>();
            VertexBuffer.Init(BufferTarget.ArrayBuffer, Vertices);
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            if (VertexBuffer != null) VertexBuffer.Dispose();
        }
    }
}
