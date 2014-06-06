using System;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    [AttributeUsage(AttributeTargets.Property)]
    public class VertexAttribAttribute
        : Attribute
    {
        public int Components { get; protected set; }
        public VertexAttribPointerType Type { get; protected set; }
        public bool Normalized { get; set; }

        public VertexAttribAttribute(int components, VertexAttribPointerType type)
        {
            Components = components;
            Type = type;
            Normalized = false;
        }
    }
}