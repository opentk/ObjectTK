using System;
using log4net;
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
        public readonly int Location;

        /// <summary>
        /// Specifies whether the uniform is active.<br/>
        /// Unused uniforms are generally removed by OpenGL and cause them to be inactive.
        /// </summary>
        public readonly bool Active;
        
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

        internal Uniform(int program, string name, Action<int, T> setter)
            : base(program, name)
        {
            Location = GL.GetUniformLocation(program, name);
            Active = Location != -1;
            if (!Active) Logger.WarnFormat("Uniform not found or not active: {0}", name);
            _setter = setter;
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