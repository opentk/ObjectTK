//
// VertexAttrib.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System.Linq;
using System.Reflection;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Represents a vertex attribute.
    /// </summary>
    public sealed class VertexAttrib
        : ProgramVariable
    {
        private static readonly Logging.IObjectTKLogger Logger = Logging.LogFactory.GetLogger(typeof(VertexAttrib));

        /// <summary>
        /// The vertex attributes location within the shader.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The number components to read.
        /// </summary>
        public int Components { get; private set; }

        /// <summary>
        /// The type of each component.
        /// </summary>
        public VertexAttribPointerType Type { get; private set; }

        /// <summary>
        /// Specifies whether the components should be normalized.
        /// </summary>
        public bool Normalized { get; private set; }

        internal VertexAttrib()
        {
        }

        internal override void Initialize(Program program, PropertyInfo property)
        {
            base.Initialize(program, property);
            var attribute = property.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault() ?? new VertexAttribAttribute();
            Components = attribute.Components;
            Type = attribute.Type;
            Normalized = attribute.Normalized;
            if (attribute.Index > 0) BindAttribLocation(attribute.Index);
        }

        public void BindAttribLocation(int index)
        {
            Index = index;
            GL.BindAttribLocation(ProgramHandle, index, Name);
        }

        internal override void OnLink()
        {
            Index = GL.GetAttribLocation(ProgramHandle, Name);
            Active = Index > -1;
            if (!Active) Logger?.WarnFormat("Vertex attribute not found or not active: {0}", Name);
        }
    }
}