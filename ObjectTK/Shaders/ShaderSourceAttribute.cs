#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders
{
    /// <summary>
    /// Specifies a source file which contains a single shader of predefined type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ShaderSourceAttribute
        : Attribute
    {
        /// <summary>
        /// Specifies the type of shader.
        /// </summary>
        public ShaderType Type { get; private set; }

        /// <summary>
        /// Specifies the effect key for this shader.<br/>
        /// Example: Path/to/file/CoolShader.Fragment.Diffuse
        /// </summary>
        public string EffectKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ShaderSourceAttribute.
        /// </summary>
        /// <param name="type">Specifies the type of the shader.</param>
        /// <param name="effectKey">Specifies the effect key for this shader.</param>
        protected ShaderSourceAttribute(ShaderType type, string effectKey)
        {
            Type = type;
            EffectKey = effectKey;
        }

        /// <summary>
        /// Retrieves all shader sources from attributes tagged to the given program type.
        /// </summary>
        /// <param name="programType">Specifies the type of the program of which the shader sources are to be found.</param>
        /// <returns>A mapping of ShaderType and source path.</returns>
        public static List<ShaderSourceAttribute> GetShaderSources(Type programType)
        {
            return programType.GetCustomAttributes<ShaderSourceAttribute>(true).ToList();
        }
    }
}