#region License
// ObjectTK License
// Copyright (C) 2013-2015 J.C.Bernack
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion
using System.Reflection;

namespace ObjectTK.Shaders.Variables
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
        public int ProgramHandle { get { return Program.Handle; } }

        /// <summary>
        /// The name of this shader variable.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Specifies whether this variable is active.<br/>
        /// Unused variables are generally removed by OpenGL and cause them to be inactive.
        /// </summary>
        public bool Active { get; protected set; }

        /// <summary>
        /// Initializes this instance using the given Program and PropertyInfo.
        /// </summary>
        internal virtual void Initialize(Program program, PropertyInfo property)
        {
            Program = program;
            Name = property.Name;
        }

        /// <summary>
        /// When overridden in a derived class, handles initialization which must occur after the program object is linked.
        /// </summary>
        internal virtual void OnLink() { }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Program.Name, Name);
        }
    }
}