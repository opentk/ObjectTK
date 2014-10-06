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

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader variable identified by its name and the corresponding program handle.
    /// </summary>
    public abstract class ShaderVariable
    {
        /// <summary>
        /// The handle of the program to which this variable relates.
        /// </summary>
        public readonly int Program;

        /// <summary>
        /// The name of this shader variable.
        /// </summary>
        public readonly string Name;

        internal ShaderVariable(int program, string name)
        {
            Program = program;
            Name = name;
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