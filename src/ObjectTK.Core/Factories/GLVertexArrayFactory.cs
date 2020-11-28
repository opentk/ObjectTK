using System;
using System.ComponentModel;
using JetBrains.Annotations;
using ObjectTK.GLObjects;
using OpenTK.Graphics.OpenGL;
using Buffer = ObjectTK.GLObjects.Buffer;

// ReSharper disable once CheckNamespace
namespace ObjectTK {
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public sealed class GLVertexArrayFactory {
        public static GLVertexArrayFactory Instance { get; } = new GLVertexArrayFactory();
        private GLVertexArrayFactory() { }
        
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
        public VertexArray FromBuffers([NotNull] string name, [NotNull] params Buffer[] buffers) {
            var length = buffers[0].ElementCount;
            #if DEBUG
            for (int i = 0; i < buffers.Length; i++) {
                var b = buffers[i];
                if (b.ElementCount != length) {
                    throw new ArgumentException($"The provided buffers must have the same number of elements.\n" +
                                                $"The buffer {b.Name} with length {b.ElementCount} did not match the expected length of {length}");
                }
            }
            #endif
            
            var vao = GL.GenVertexArray();
            var label = $"VertexArray: {name}";
            GL.BindVertexArray(vao);
            GL.ObjectLabel(ObjectLabelIdentifier.VertexArray,vao, name.Length, label);
            for (int i = 0; i < buffers.Length; i++) {
                var buffer = buffers[i];
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, buffer.ComponentCount, buffer.AttribType, false, buffer.ElementSize,0);
            }
            // clean up:
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            
            return new VertexArray(name, vao, buffers, null);
        }
        
        /// Creates a vertex array from the provided index and vertex buffers.
        [Pure]
        [NotNull]
        public VertexArray IndexAndVertexBuffers([NotNull] string name, Buffer<int> indexBuffer, params Buffer[] vertexBuffers) {
            var b = FromBuffers(name, vertexBuffers);
            GL.BindVertexArray(b.Handle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.Handle);
            GL.BindVertexArray(0);
            b.IndexBuffer = indexBuffer;
            return b;
        }
    }
}
