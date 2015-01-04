#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using System;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Represents a uniform.
    /// </summary>
    /// <typeparam name="T">The type of the uniform.</typeparam>
    public class Uniform<T>
        : ProgramVariable
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
        private readonly Action<int, T> _setter;

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

        internal Uniform()
            : this(UniformSetter.Get<T>())
        {
        }

        public Uniform(Action<int, T> setter)
        {
            if (setter == null) throw new ArgumentNullException("setter");
            _setter = setter;
        }

        internal override void OnLink()
        {
            Location = GL.GetUniformLocation(ProgramHandle, Name);
            Active = Location > -1;
            if (!Active) Logger.WarnFormat("Uniform not found or not active: {0}", Name);
        }

        /// <summary>
        /// Sets the given value to the program uniform.<br/>
        /// Must be called on an active program, i.e. after <see cref="Program"/>.<see cref="Program.Use()"/>.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void Set(T value)
        {
            Program.AssertActive();
            _value = value;
            if (Active) _setter(Location, value);
        }
    }
}