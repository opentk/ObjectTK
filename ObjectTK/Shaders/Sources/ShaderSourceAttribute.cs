//
// ShaderSourceAttribute.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Sources
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