using System;

namespace SphFluid.Core.Shaders
{
    public class ShaderUniform<T>
    {
        public readonly int Location;
        private readonly Action<int, T> _setter;

        public ShaderUniform(int location, Action<int, T> setter)
        {
            Location = location;
            _setter = setter;
        }

        public void Set(T value)
        {
            _setter(Location, value);
        }
    }
}