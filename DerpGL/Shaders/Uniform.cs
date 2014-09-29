using System;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    public class Uniform<T>
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Uniform<T>));

        public readonly int Location;
        public readonly bool Active;
        private readonly Action<int, T> _setter;

        private T _value;
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

        public Uniform(int program, string name, Action<int, T> setter)
            : base(program, name)
        {
            Location = GL.GetUniformLocation(program, name);
            Active = Location != -1;
            if (!Active) Logger.WarnFormat("Uniform not found or not active: {0}", name);
            _setter = setter;
        }

        public bool Set(T value)
        {
            _value = value;
            if (!Active) return false;
            _setter(Location, value);
            return true;
        }
    }
}