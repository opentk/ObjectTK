using System;
using System.Reflection;

namespace DerpGL.Shaders
{
    public class Mapping<T>
        : IMapping
    {
        private readonly Func<int, MemberInfo, T> _creator;

        public Type MappedType { get { return typeof(T); } }

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