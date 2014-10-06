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

namespace DerpGL
{
    /// <summary>
    /// Represents an OpenGL handle.
    /// </summary>
    public class GLObject
        : IEquatable<GLObject>
    {
        /// <summary>
        /// The OpenGL handle.
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// Initializes a new instance of the GLObject class.
        /// </summary>
        public GLObject(int handle)
        {
            Handle = handle;
        }

        public bool Equals(GLObject other)
        {
            return other != null && Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            return obj is GLObject && Equals(obj as GLObject);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", GetType().Name, Handle);
        }
    }
}