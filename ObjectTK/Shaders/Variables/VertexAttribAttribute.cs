//
// VertexAttribAttribute.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Defines default values when binding buffers to the attributed <see cref="VertexAttrib"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class VertexAttribAttribute
        : Attribute
    {
        /// <summary>
        /// The number components to read.<br/>
        /// Defaults to 4.
        /// </summary>
        public int Components { get; protected set; }

        /// <summary>
        /// The type of each component.<br/>
        /// Defaults to Float.
        /// </summary>
        public VertexAttribPointerType Type { get; protected set; }
        
        /// <summary>
        /// Specifies whether the components should be normalized.<br/>
        /// Defaults to false.
        /// </summary>
        public bool Normalized { get; protected set; }

        /// <summary>
        /// Specifies the generic vertex attribute index this named attribute variable binds to.<br/>
        /// If set explicitly this named attribute variable is bound to that generic vertex attribute index before the program is linked.<br/>
        /// Defaults to -1, which causes the index to be retrieved after the program is linked.
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute with default values.<br/>
        /// The default is 4 components of type float without normalization.
        /// </summary>
        public VertexAttribAttribute()
            : this(4, VertexAttribPointerType.Float, false, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute.<br/>
        /// Normalization defaults to false.
        /// </summary>
        /// <param name="components">The number of components to read.</param>
        /// <param name="type">The type of each component.</param>
        public VertexAttribAttribute(int components, VertexAttribPointerType type)
            : this(components, type, false, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute.
        /// </summary>
        /// <param name="components">The number of components to read.</param>
        /// <param name="type">The type of each component.</param>
        /// <param name="normalized">Specifies whether each component should be normalized.</param>
        public VertexAttribAttribute(int components, VertexAttribPointerType type, bool normalized)
            : this(components, type, normalized, -1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute.
        /// </summary>
        /// <param name="components">The number of components to read.</param>
        /// <param name="type">The type of each component.</param>
        /// <param name="normalized">Specifies whether each component should be normalized.</param>
        /// <param name="index"></param>
        public VertexAttribAttribute(int components, VertexAttribPointerType type, bool normalized, int index)
        {
            Components = components;
            Type = type;
            Normalized = normalized;
            Index = index;
        }
    }
}