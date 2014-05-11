using System;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shaders
{
    public class Uniform<T>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Uniform<T>));

        public readonly int Location;
        private readonly Action<int, T> _setter;

        public Uniform(int program, string name, Action<int, T> setter)
        {
            Location = GL.GetUniformLocation(program, name);
            if (Location == -1) Logger.WarnFormat("Uniform not found or not active: {0}", name);
            _setter = setter;
        }

        public void Set(T value)
        {
            _setter(Location, value);
        }
    }
}