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