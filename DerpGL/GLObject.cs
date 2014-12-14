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
    /// Represents an OpenGL handle.<br/>
    /// Must be disposed explicitly, otherwise there will be a memory leak which will be logged as a warning.
    /// </summary>
    public abstract class GLObject
        : GLResource
        , IEquatable<GLObject>
    {
        /// <summary>
        /// The OpenGL handle.
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// Initializes a new instance of the GLResource class.
        /// </summary>
        protected GLObject(int handle)
        {
            Handle = handle;
        }

        public bool Equals(GLObject other)
        {
            return other != null && Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            return obj is GLObject && Equals((GLObject) obj);
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