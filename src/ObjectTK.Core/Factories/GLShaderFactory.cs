using System;
using System.ComponentModel;
using JetBrains.Annotations;
using ObjectTK.GLObjects;
using ObjectTK.Shaders;

// ReSharper disable once CheckNamespace
namespace ObjectTK {
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
}
