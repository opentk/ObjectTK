//
// UniformSetter.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ObjectTK.Exceptions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    internal abstract class UniformSetter
    {
        protected abstract Type MappedType { get; }

        private class Map<T>
            : UniformSetter
        {
            protected override Type MappedType { get { return typeof(T); } }

            public readonly Action<int, T> Setter;

            public Map(Action<int, T> setter)
            {
                Setter = setter;
            }
        }

        private UniformSetter() { }

        private static readonly List<UniformSetter> Setters = new List<UniformSetter>();

        static UniformSetter()
        {
            Setters = new List<UniformSetter>
            {
                new Map<bool>((_,value) => GL.Uniform1(_, value ? 1 : 0)),
                new Map<int>(GL.Uniform1),
                new Map<uint>(GL.Uniform1),
                new Map<float>(GL.Uniform1),
                new Map<double>(GL.Uniform1),
                new Map<Half>((_, half) => GL.Uniform1(_, half)),
                new Map<Color>((_, color) => GL.Uniform4(_, color)),
                new Map<Vector2>(GL.Uniform2),
                new Map<Vector3>(GL.Uniform3),
                new Map<Vector4>(GL.Uniform4),
                new Map<Vector2d>((_, vector) => GL.Uniform2(_, vector.X, vector.Y)),
                new Map<Vector2h>((_, vector) => GL.Uniform2(_, vector.X, vector.Y)),
                new Map<Vector3d>((_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z)),
                new Map<Vector3h>((_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z)),
                new Map<Vector4d>((_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W)),
                new Map<Vector4h>((_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W)),
                new Map<Matrix2>((_, matrix) => GL.UniformMatrix2(_, false, ref matrix)),
                new Map<Matrix3>((_, matrix) => GL.UniformMatrix3(_, false, ref matrix)),
                new Map<Matrix4>((_, matrix) => GL.UniformMatrix4(_, false, ref matrix)),
                new Map<Matrix2x3>((_, matrix) => GL.UniformMatrix2x3(_, false, ref matrix)),
                new Map<Matrix2x4>((_, matrix) => GL.UniformMatrix2x4(_, false, ref matrix)),
                new Map<Matrix3x2>((_, matrix) => GL.UniformMatrix3x2(_, false, ref matrix)),
                new Map<Matrix3x4>((_, matrix) => GL.UniformMatrix3x4(_, false, ref matrix)),
                new Map<Matrix4x2>((_, matrix) => GL.UniformMatrix4x2(_, false, ref matrix)),
                new Map<Matrix4x3>((_, matrix) => GL.UniformMatrix4x3(_, false, ref matrix))
            };
        }

        public static Action<int, T> Get<T>()
        {
            var setter = Setters.FirstOrDefault(_ => _.MappedType == typeof(T));
            if (setter == null) throw new UniformTypeNotSupportedException(typeof(T));
            return ((Map<T>)setter).Setter;
        }
    }
}