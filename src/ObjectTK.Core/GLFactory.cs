using System;
using System.ComponentModel;
using JetBrains.Annotations;
using ObjectTK.GLObjects;
using ObjectTK.Internal;
using ObjectTK.Shaders;
using OpenTK.Graphics.OpenGL;
using Buffer = ObjectTK.GLObjects.Buffer;

namespace ObjectTK {

    namespace Internal {
        
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public sealed class GLShaderFactory {
            public static GLShaderFactory Instance { get; } = new GLShaderFactory();
            private GLShaderFactory() { }

            [MustUseReturnValue]
            [NotNull]
            public ShaderProgram VertexFrag(string name, string vertexSource, string fragSource) {
                return ShaderCompiler.VertexFrag(name, vertexSource, fragSource);
            }
            
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
            public new Type GetType() {
                return base.GetType();
            }
        }
        
        
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public sealed class GLTextureFactory {
            public static GLTextureFactory Instance { get; } = new GLTextureFactory();
            private GLTextureFactory() { }
            
            /// Creates a texture from a raw pointer.
            /// This is typically used for creation from a bitmap.
            [NotNull]
            [MustUseReturnValue]
            public Texture2D Create2D(string name, [NotNull] TextureConfig cfg, int width, int height, IntPtr data) {
                var t = GL.GenTexture();
                var label = $"Texture2D: {name}";
                GL.BindTexture(TextureTarget.Texture2D, t);
                GL.TexImage2D(TextureTarget.Texture2D, 0,cfg.InternalFormat, width, height, 0, cfg.PixelFormat, cfg.PixelType, data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) cfg.MagFilter);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) cfg.MinFilter);
                GL.ObjectLabel(ObjectLabelIdentifier.Texture, t, label.Length, label);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                return new Texture2D(t, name, cfg.InternalFormat, width, height);
            }

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
            public new Type GetType() {
                return base.GetType();
            }
        }
        
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
        }
    
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
                
                return new VertexArray(name, vao, length, false);
            }
            
            /// Creates a vertex array from the 
            [Pure]
            [NotNull]
            public VertexArray IndexAndVertexBuffers([NotNull] string name, Buffer<int> indexBuffer, params Buffer[] vertexBuffers) {
                var b = FromBuffers(name, vertexBuffers);
                GL.BindVertexArray(b.Handle);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.Handle);
                GL.BindVertexArray(0);
                b.ElementCount = indexBuffer.ElementCount;
                b.HasElementArrayBuffer = true;
                return b;
            }
        }
    }

    /// Top-level class responsible for creating all OpenGL objects.
    /// ------
    /// Usage: GLFactory.Shader.VertexFrag() 
    public static class GLFactory {

        public static readonly GLShaderFactory Shader = GLShaderFactory.Instance;
        public static readonly GLBufferFactory Buffer = GLBufferFactory.Instance;
        public static readonly GLVertexArrayFactory VertexArray = GLVertexArrayFactory.Instance;
        public static readonly GLTextureFactory Texture = GLTextureFactory.Instance;
    }
    
}
