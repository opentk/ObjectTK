//
// Uniform.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
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
        private static readonly Logging.IObjectTKLogger Logger = Logging.LogFactory.GetLogger(typeof(Uniform<T>));

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
            if (!Active) Logger?.WarnFormat("Uniform not found or not active: {0}", Name);
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