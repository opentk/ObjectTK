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
using System.Linq;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a vertex attribute.
    /// </summary>
    public sealed class VertexAttrib
        : ProgramVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VertexAttrib));

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

        protected override void Initialize()
        {
            var attribute = Property.GetCustomAttributes<VertexAttribAttribute>(false).FirstOrDefault() ?? new VertexAttribAttribute();
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
            if (!Active) Logger.WarnFormat("Vertex attribute not found or not active: {0}", Name);
        }
    }
}