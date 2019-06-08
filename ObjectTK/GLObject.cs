//
// GLObject.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;

namespace ObjectTK
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