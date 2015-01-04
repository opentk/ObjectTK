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