using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Shaders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexAttribAttribute
        : Attribute
    {
        public readonly int Components;
        public readonly VertexAttribPointerType Type;

        public VertexAttribAttribute(int components, VertexAttribPointerType type)
        {
            Components = components;
            Type = type;
        }
    }
}