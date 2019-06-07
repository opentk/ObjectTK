//
// UniformTypeNotSupportedException.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Runtime.Serialization;
using ObjectTK.Shaders.Variables;

namespace ObjectTK.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the generic type parameter used for an instance of <see cref="Uniform{T}"/> is not supported.
    /// </summary>
    [Serializable]
    public class UniformTypeNotSupportedException
        : ObjectTKException
    {
        /// <summary>
        /// The unsupported type parameter to <see cref="Uniform{T}"/> which caused the initialization to fail.
        /// </summary>
        public readonly Type UniformType;

        internal UniformTypeNotSupportedException(Type uniformType)
            : base(string.Format("Uniforms of type {0} are not supported", uniformType.Name))
        {
            UniformType = uniformType;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("UniformType", UniformType);
        }
    }
}