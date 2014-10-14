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
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders.Variables
{
    /// <summary>
    /// Represents a fragment shader output.<br/>
    /// TODO: implement methods to bind output to a specific attachment
    /// see glBindFragDataLocation, glDrawBuffers and http://stackoverflow.com/questions/1733838/fragment-shaders-output-variables
    /// </summary>
    public sealed class FragData
        : ShaderVariable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FragData));

        /// <summary>
        /// The location of the output.
        /// </summary>
        public int Location { get; private set; }

        internal FragData()
        {
        }

        internal override void OnLink()
        {
            //TODO: find out what GL.GetFragDataIndex(); does
            Location = GL.GetFragDataLocation(ProgramHandle, Name);
            if (Location == -1) Logger.WarnFormat("Output variable not found or not active: {0}", Name);
        }
    }
}