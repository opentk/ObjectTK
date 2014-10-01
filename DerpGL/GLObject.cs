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