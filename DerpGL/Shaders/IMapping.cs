using System;
using System.Reflection;

namespace DerpGL.Shaders
{
    internal interface IMapping
    {
        Type MappedType { get; }
        object Create(int program, MemberInfo info);
    }
}