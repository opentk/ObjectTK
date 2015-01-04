#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Tools.Shapes
{
    public abstract class TexturedShape
        : Shape
    {
        public Vector2[] TexCoords { get; protected set; }
        public Buffer<Vector2> TexCoordBuffer { get; protected set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            TexCoordBuffer = new Buffer<Vector2>();
            TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (TexCoordBuffer != null) TexCoordBuffer.Dispose();
        }
    }
}