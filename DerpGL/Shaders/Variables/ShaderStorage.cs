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
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a shader storage buffer object (SSBO) binding.
    /// </summary>
    public sealed class ShaderStorage
        : BufferBinding
    {
        internal ShaderStorage(int program, string name)
            : base(program, name, BufferRangeTarget.ShaderStorageBuffer, ProgramInterface.ShaderStorageBlock)
        {
            //TODO: find out if the current binding point can be queried, like it can be for uniform blocks
            // set the binding point to the blocks index
            if (Active) ChangeBinding(Index);
        }

        /// <summary>
        /// Assigns a binding point to this shader storage block.
        /// </summary>
        /// <param name="binding"></param>
        public override void ChangeBinding(int binding)
        {
            base.ChangeBinding(binding);
            GL.ShaderStorageBlockBinding(Program, Index, binding);
        }
    }
}