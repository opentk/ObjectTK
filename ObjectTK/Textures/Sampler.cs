//
// Sampler.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Textures
{
    /// <summary>
    /// Represents a sampler object.
    /// </summary>
    public sealed class Sampler
        : GLObject
    {
        /// <summary>
        /// Initializes a new sampler object.
        /// </summary>
        public Sampler()
            : base(GL.GenSampler())
        {
        }

        protected override void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteSampler(Handle);
        }

        /// <summary>
        /// Binds the sampler to the given texture unit.
        /// </summary>
        /// <param name="textureUnit">The texture unit to bind to.</param>
        public void Bind(TextureUnit textureUnit)
        {
            Bind((int)textureUnit - (int)TextureUnit.Texture0);
        }

        /// <summary>
        /// Binds the sampler to the given texture unit.
        /// </summary>
        /// <param name="unit">The texture unit to bind to.</param>
        public void Bind(int unit)
        {
            GL.BindSampler(unit, Handle);
        }

        /// <summary>
        /// Sets the given wrap mode on all dimensions R, S and T.
        /// </summary>
        /// <param name="wrapMode">The wrap mode to apply.</param>
        public void SetWrapMode(TextureWrapMode wrapMode)
        {
            var mode = (int) wrapMode;
            SetParameter(SamplerParameterName.TextureWrapR, mode);
            SetParameter(SamplerParameterName.TextureWrapS, mode);
            SetParameter(SamplerParameterName.TextureWrapT, mode);
        }

        /// <summary>
        /// Sets sampler parameters.
        /// </summary>
        /// <param name="parameterName">The parameter name to set.</param>
        /// <param name="value">The value to set.</param>
        public void SetParameter(SamplerParameterName parameterName, int value)
        {
            GL.SamplerParameter(Handle, parameterName, value);
        }
    }
}