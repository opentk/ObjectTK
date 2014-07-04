using System;

namespace DerpGL
{
    public class GLObject
        : IEquatable<GLObject>
    {
        public readonly int Handle;

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