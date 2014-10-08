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
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Defines default values when binding buffers to the attributed <see cref="VertexAttrib"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class VertexAttribAttribute
        : Attribute
    {
        /// <summary>
        /// The number components to read.
        /// </summary>
        public int Components { get; protected set; }

        /// <summary>
        /// The type of each component.
        /// </summary>
        public VertexAttribPointerType Type { get; protected set; }
        
        /// <summary>
        /// Specifies whether the components should be normalized.
        /// </summary>
        public bool Normalized { get; protected set; }

        public int Location { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute with default values.<br/>
        /// The default is 4 components of type float without normalization.
        /// </summary>
        public VertexAttribAttribute()
            : this(4, VertexAttribPointerType.Float, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute.<br/>
        /// Normalization defaults to false.
        /// </summary>
        /// <param name="components">The number of components to read.</param>
        /// <param name="type">The type of each component.</param>
        public VertexAttribAttribute(int components, VertexAttribPointerType type)
            : this(components, type, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute.
        /// </summary>
        /// <param name="components">The number of components to read.</param>
        /// <param name="type">The type of each component.</param>
        /// <param name="normalized">Specifies whether each component should be normalized.</param>
        public VertexAttribAttribute(int components, VertexAttribPointerType type, bool normalized)
        {
            Components = components;
            Type = type;
            Normalized = normalized;
            Location = -1;
        }
    }
}