using System;
using System.Reflection;

namespace DerpGL.Shaders.Variables
{
    public interface IShaderVariableCallback
    {
        Type MappedType { get; }
        void Invoke(ShaderVariable property, PropertyInfo propertyInfo);
    }
}