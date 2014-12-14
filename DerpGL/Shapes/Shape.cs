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
using System;
using DerpGL.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
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
    }
}
