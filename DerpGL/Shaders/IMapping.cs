#region License
// DerpGL License
// Copyright (C) 2013-2014 J.C.Bernack
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
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