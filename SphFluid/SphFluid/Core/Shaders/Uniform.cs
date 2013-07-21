using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shaders
{
    public class Uniform<T>
    {
        public readonly int Location;
        private readonly Action<int, T> _setter;

        public Uniform(int program, string name, Action<int, T> setter)
        {
            Location = GL.GetUniformLocation(program, name);
            if (Location == -1) Trace.TraceWarning(string.Format("Uniform not found or not active: {0}", name));
            _setter = setter;
        }

        public void Set(T value)
        {
            _setter(Location, value);
        }
    }
}