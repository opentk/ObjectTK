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
namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// TODO: Implemented in a completely strange way ..
    /// </summary>
    public sealed class TransformOut
        : ProgramVariable
    {
        public int Index { get; internal set; }

        public TransformOut()
        {
            Index = -1;
            Active = false;
        }

        /// <summary>
        /// Initialized a dummy instance of TransformOut used for the keywords introduced with advanced interleaving.
        /// </summary>
        /// <param name="name"></param>
        internal TransformOut(string name)
        {
            Name = name;
        }
    }
}