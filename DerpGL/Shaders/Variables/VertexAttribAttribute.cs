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
        }
    }
}