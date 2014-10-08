using System;
using System.Reflection;

namespace DerpGL.Shaders.Variables
{
    public sealed class ShaderVariableCallback<T>
        : IShaderVariableCallback
        where T : ShaderVariable
    {
        private readonly Action<T, PropertyInfo> _callback;

        public Type MappedType { get { return typeof(T); } }

        public ShaderVariableCallback(Action<T, PropertyInfo> callback)
        {
            _callback = callback;
        }

        public void Invoke(ShaderVariable property, PropertyInfo propertyInfo)
        {
            _callback.Invoke((T)property, propertyInfo);
        }
    }
}