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
using System.Reflection;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader variable identified by its name and the corresponding program handle.
    /// </summary>
    public abstract class ProgramVariable
    {
        /// <summary>
        /// The handle of the program to which this variable relates.
        /// </summary>
        protected Program Program { get; private set; }

        /// <summary>
        /// The handle of the program to which this variable relates.
        /// </summary>
        protected PropertyInfo Property { get; private set; }

        /// <summary>
        /// The handle of the program to which this variable relates.
        /// </summary>
        public int ProgramHandle { get { return Program.Handle; } }

        /// <summary>
        /// The name of this shader variable.
        /// </summary>
        public string Name { get { return Property.Name; } }

        /// <summary>
        /// Specifies whether this variable is active.<br/>
        /// Unused variables are generally removed by OpenGL and cause them to be inactive.
        /// </summary>
        public bool Active { get; protected set; }

        /// <summary>
        /// Initializes this instance using the given Program and PropertyInfo.
        /// </summary>
        internal void Initialize(Program program, PropertyInfo property)
        {
            Program = program;
            Property = property;
            Initialize();
        }

        /// <summary>
        /// When overridden in a derived class, handles initialization which depends on Program and PropertyInfo,
        /// which are not available at construction.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// When overridden in a derived class, handles initialization which must occur after the program object is linked.
        /// </summary>
        internal virtual void OnLink() { }
    }
}