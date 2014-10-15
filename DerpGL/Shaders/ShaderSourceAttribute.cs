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
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Specifies a shader type and the path to its source.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ShaderSourceAttribute
        : Attribute
    {
        /// <summary>
        /// Specifies the type of shader.
        /// </summary>
        public ShaderType Type { get; private set; }

        /// <summary>
        /// Specifies the path and filename to the source file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ShaderSourceAttribute.
        /// </summary>
        /// <param name="type">Specifies the type of the shader.</param>
        /// <param name="path">Specifies the path to the source file.</param>
        public ShaderSourceAttribute(ShaderType type, string path)
        {
            Type = type;
            Path = path;
        }

        /// <summary>
        /// Retrieves all shader sources from attributes tagged to the given program type.
        /// </summary>
        /// <param name="programType">Specifies the type of the program of which the shader sources are to be found.</param>
        /// <returns>A mapping of ShaderType and source path.</returns>
        public static Dictionary<ShaderType, string> GetShaderSources(Type programType)
        {
            return programType.GetCustomAttributes<ShaderSourceAttribute>(true).ToDictionary(_ => _.Type, _ => _.Path);
        }
    }
}