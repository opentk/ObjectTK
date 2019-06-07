//
// TessControlShaderSourceAttribute.cs
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
    /// Specifies the source of a vertex shader.
    /// </summary>
    public class TessControlShaderSourceAttribute
        : ShaderSourceAttribute
    {
        /// <summary>
        /// Initializes a new instance of the TessControlShaderSourceAttribute.
        /// </summary>
        /// <param name="effectKey">Specifies the effect key for this shader.</param>
        public TessControlShaderSourceAttribute(string effectKey)
            : base(ShaderType.TessControlShader, effectKey)
        {
        }
    }
}