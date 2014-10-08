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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader variable identified by its name and the corresponding program handle.
    /// </summary>
    public abstract class ShaderVariable
    {
        private static readonly List<IShaderVariableCallback> TypedCallbacks =  new List<IShaderVariableCallback>();

        protected static void AddTypedCallback(IShaderVariableCallback callback)
        {
            TypedCallbacks.Add(callback);
        }

        internal static void InvokeCallback<T>(T property, PropertyInfo propertyInfo)
            where T : ShaderVariable
        {
            foreach (var callback in TypedCallbacks.Where(callback => callback.MappedType == property.GetType()))
            {
                callback.Invoke(property, propertyInfo);
                return;
            }
        }

        /// <summary>
        /// The handle of the program to which this variable relates.
        /// </summary>
        public int Program { get; private set; }

        /// <summary>
        /// The name of this shader variable.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Specifies whether this variable is active.<br/>
        /// Unused variables are generally removed by OpenGL and cause them to be inactive.
        /// </summary>
        public bool Active { get; protected set; }

        protected event Action PreLink;

        protected event Action PostLink;

        /// <summary>
        /// Called right after instantiation.
        /// </summary>
        internal void Initialize(int program, string name)
        {
            Program = program;
            Name = name;
        }

        /// <summary>
        /// Called after attaching the shaders, but before linking the program.
        /// </summary>
        internal virtual void FirePreLink()
        {
            var handler = PreLink;
            if (handler != null) handler();
        }

        /// <summary>
        /// Called after all shaders are attached and the program is linked.
        /// </summary>
        internal virtual void FirePostLink()
        {
            var handler = PostLink;
            if (handler != null) handler();
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if this shader is not the currently active one.
        /// </summary>
        protected void AssertProgramActive()
        {
            if (Shader.ActiveProgramHandle != Program) throw new InvalidOperationException("Can not set uniforms of an inactive program. Call Shader.Use() before setting any uniforms.");
        }
    }
}