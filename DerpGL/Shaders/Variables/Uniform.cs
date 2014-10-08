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
using System.Drawing;
using System.Threading;
using log4net;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a uniform.
    /// </summary>
    /// <typeparam name="T">The type of the uniform.</typeparam>
    public class Uniform<T>
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Uniform<T>));

        /// <summary>
        /// The location of the uniform within the shader program.
        /// </summary>
        public int Location { get; private set; }
        
        /// <summary>
        /// Action used to set the uniform.<br/>
        /// Inputs are the uniforms location and the value to set.
        /// </summary>
        private Action<int, T> _setter;

        /// <summary>
        /// The current value of the uniform.
        /// </summary>
        private T _value;

        /// <summary>
        /// Gets or sets the current value of the shader uniform.
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                Set(value);
            }
        }

        protected static void Register<TU>(Action<int, TU> setter)
        {
            AddTypedCallback(new ShaderVariableCallback<Uniform<TU>>((_,i) => _._setter = setter));
        }

        static Uniform()
        {
            Register<bool>((_,value) => GL.Uniform1(_, value ? 1 : 0));
            Register<int>(GL.Uniform1);
            Register<uint>(GL.Uniform1);
            Register<float>(GL.Uniform1);
            Register<double>(GL.Uniform1);
            Register<Half>((_, half) => GL.Uniform1(_, half));
            Register<Color>((_, color) => GL.Uniform4(_, color));
            Register<Vector2>(GL.Uniform2);
            Register<Vector3>(GL.Uniform3);
            Register<Vector4>(GL.Uniform4);
            Register<Vector2d>((_, vector) => GL.Uniform2(_, vector.X, vector.Y));
            Register<Vector2h>((_, vector) => GL.Uniform2(_, vector.X, vector.Y));
            Register<Vector3d>((_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z));
            Register<Vector3h>((_, vector) => GL.Uniform3(_, vector.X, vector.Y, vector.Z));
            Register<Vector4d>((_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W));
            Register<Vector4h>((_, vector) => GL.Uniform4(_, vector.X, vector.Y, vector.Z, vector.W));
            Register<Matrix2>((_, matrix) => GL.UniformMatrix2(_, false, ref matrix));
            Register<Matrix3>((_, matrix) => GL.UniformMatrix3(_, false, ref matrix));
            Register<Matrix4>((_, matrix) => GL.UniformMatrix4(_, false, ref matrix));
            Register<Matrix2x3>((_, matrix) => GL.UniformMatrix2x3(_, false, ref matrix));
            Register<Matrix2x4>((_, matrix) => GL.UniformMatrix2x4(_, false, ref matrix));
            Register<Matrix3x2>((_, matrix) => GL.UniformMatrix3x2(_, false, ref matrix));
            Register<Matrix3x4>((_, matrix) => GL.UniformMatrix3x4(_, false, ref matrix));
            Register<Matrix4x2>((_, matrix) => GL.UniformMatrix4x2(_, false, ref matrix));
            Register<Matrix4x3>((_, matrix) => GL.UniformMatrix4x3(_, false, ref matrix));
        }

        public Uniform(Action<int, T> setter)
            : this()
        {
            _setter = setter;
        }

        internal Uniform()
        {
            PreLink += OnPreLink;
            PostLink += OnPostLink;
        }

        private void OnPreLink()
        {
            if (_setter == null) throw new ApplicationException("This type of uniform is not supported: " + typeof(T).FullName);
        }

        internal void OnPostLink()
        {
            Location = GL.GetUniformLocation(Program, Name);
            Active = Location != -1;
            if (!Active) Logger.WarnFormat("Uniform not found or not active: {0}", Name);
        }

        /// <summary>
        /// Sets the given value to the program uniform.<br/>
        /// Must be called on an active shader, i.e. after <see cref="Shader"/>.<see cref="Shader.Use()"/>, otherwise an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>True if the uniform was successfully set, otherwise false.</returns>
        public bool Set(T value)
        {
            AssertProgramActive();
            _value = value;
            if (!Active) return false;
            _setter(Location, value);
            return true;
        }
    }
}