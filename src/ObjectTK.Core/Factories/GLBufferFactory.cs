using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using ObjectTK.GLObjects;
using ObjectTK.Internal;
using OpenTK.Graphics.OpenGL;

// ReSharper disable once CheckNamespace
namespace ObjectTK {
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class GLBufferFactory {
        public static GLBufferFactory Instance { get; } = new GLBufferFactory();
        private GLBufferFactory() { }
        
        // Hide the default members of this object for a cleaner API.
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            // ReSharper disable once BaseObjectEqualsIsObjectEquals
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        // ReSharper disable once AnnotateCanBeNullTypeMember
        public override string ToString()
        {
            return base.ToString();
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [NotNull]
        // ReSharper disable once UnusedMember.Global
        public new Type GetType() {
            return base.GetType();
        }


        [NotNull]
        [MustUseReturnValue]
        public Buffer<T> ArrayBuffer<T>(string name, [NotNull] T[] vertices, BufferUsageHint usageHint = BufferUsageHint.StaticDraw) where T: unmanaged {
            var vbo = GL.GenBuffer();
            var label = $"Buffer: {name}";
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.ObjectLabel(ObjectLabelIdentifier.Buffer, vbo,label.Length, label);
            int elemSize;
            unsafe {
                elemSize = sizeof(T);
            }
            GL.BufferData(BufferTarget.ArrayBuffer, elemSize * vertices.Length, vertices, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return new Buffer<T>(name, vbo, vertices.Length);
        }
        
        /// Creates an array buffer from a List.
        /// This does not perform any memory copy or allocation, as this directly accesses the internal array used by the list.
        /// This has identical performance to the T[] overloads.
        [Pure]
        [MustUseReturnValue]
        public Buffer<T> ArrayBuffer<T>(string name, List<T> list, BufferUsageHint usageHint = BufferUsageHint.StaticDraw) where T : unmanaged {
            var arr = list.GetInternalArray();
            return ArrayBuffer(name, arr, usageHint);
        }
        
        /// Creates an array buffer from a List. This matches against the type, trying to find a number of fast-path options.<br></br>
        /// If those fast-paths are found, then this does not perform any memory copy or allocation as this directly accesses the internal array used by the list.
        /// In this case, this has identical performance to the T[] overloads.<br></br>
        /// Supported fast-paths are:<br></br>
        /// T[]<br></br>
        /// List&lt;T&gt;<br></br>
        [NotNull]
        [MustUseReturnValue]
        public Buffer<T> ArrayBuffer<T>(string name, IReadOnlyList<T> list, BufferUsageHint usageHint = BufferUsageHint.StaticDraw) where T : unmanaged {
            if (list is T[] arr) {
                return ArrayBuffer(name, arr, usageHint);
            }

            if (list is List<T> resizeArr) {
                return ArrayBuffer(name, resizeArr, usageHint);
            }
            // slow path, but whatever:
            var copy = new T[list.Count];
            for (var i = 0; i < list.Count; i++) {
                copy[i] = list[i];
            }
            return ArrayBuffer(name, copy, usageHint);
        }
    }
}
