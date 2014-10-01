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
        public bool Normalized { get; set; }

        /// <summary>
        /// Initializes a new instance of the VertexAttribAttribute.
        /// </summary>
        /// <param name="components">The number of components to read.</param>
        /// <param name="type">The type of each component.</param>
        public VertexAttribAttribute(int components, VertexAttribPointerType type)
        {
            Components = components;
            Type = type;
            Normalized = false;
        }
    }
}