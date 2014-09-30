using System;
using System.Reflection;

namespace DerpGL.Shaders
{
    public interface IMapping
    {
        Type MappedType { get; }
        object Create(int program, MemberInfo info);
    }
}