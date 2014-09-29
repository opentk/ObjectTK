using System;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader variable identified by its name and the corresponding program handle.
    /// </summary>
    public class ShaderVariable
    {
        /// <summary>
        /// The handle of the program to which this variable relates.
        /// </summary>
        public readonly int Program;

        /// <summary>
        /// The name of this shader variable.
        /// </summary>
        public readonly string Name;

        protected ShaderVariable(int program, string name)
        {
            Program = program;
            Name = name;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if this shader is not the currently active one.
        /// </summary>
        protected void AssertProgramActive()
        {
            if (Shader.ActiveProgramHandle != Program) throw new InvalidOperationException("Can not set uniforms of an inactive program. Call Shader.Use() before setting any uniforms.");
        }
    }
}