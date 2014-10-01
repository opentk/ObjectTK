using System;
using System.Reflection;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Defines a container to map a <see cref="Type"/> to a function capable of initializing a new instance of that type.
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// The mapped type.
        /// </summary>
        Type MappedType { get; }

        /// <summary>
        /// Initializes a new instance of the mapped type.
        /// </summary>
        /// <param name="program">The shader program handle.</param>
        /// <param name="info">The MemberInfo of the property which is initialized with the return value of this method.</param>
        /// <returns>An new initialized instance of type T.</returns>
        object Create(int program, MemberInfo info);
    }
}