//
// GeometryShaderSourceAttribute.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Sources
{
    /// <summary>
    /// Specifies the source of a geometry shader.
    /// </summary>
    public class GeometryShaderSourceAttribute
        : ShaderSourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the GeometryShaderSourceAttribute.
        /// </summary>
        /// <param name="effectKey">Specifies the effect key for this shader.</param>
        public GeometryShaderSourceAttribute(string effectKey)
            : base(ShaderType.GeometryShader, effectKey)
        {
        }
    }
}