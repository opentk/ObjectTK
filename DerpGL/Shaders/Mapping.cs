using System;
using System.Reflection;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Contains a mapping a <see cref="Type"/> to a function used to initialize an instance of that type.
    /// </summary>
    /// <typeparam name="T">The type to initialize.</typeparam>
    public class Mapping<T>
        : IMapping
    {
        private readonly Func<int, MemberInfo, T> _creator;

        public Type MappedType { get { return typeof(T); } }

        /// <summary>
        /// Creates a new mapping.
        /// </summary>
        /// <param name="creator">The function used to initialize instances of the mapped type.</param>
        public Mapping(Func<int, MemberInfo, T> creator)
        {
            _creator = creator;
        }

        public object Create(int program, MemberInfo info)
        {
            return _creator(program, info);
        }
    }
}