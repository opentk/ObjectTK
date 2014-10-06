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
using DerpGL.Buffers;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shapes
{
    public abstract class IndexedShape
        : Shape
    {
        public uint[] Indices { get; protected set; }
        public Buffer<uint> IndexBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            IndexBuffer = new Buffer<uint>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (IndexBuffer != null) IndexBuffer.Dispose();
        }
    }
}