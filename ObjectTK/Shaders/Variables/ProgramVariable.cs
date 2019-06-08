//
// ProgramVariable.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

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