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
using DerpGL.Exceptions;
using DerpGL.Shaders.Variables;
using OpenTK.Graphics.OpenGL;

namespace DerpGL.Shaders
{
    /// <summary>
    /// Represents a program object which utilizes transform feedback.
    /// </summary>
    public class TransformProgram
        : Program
    {
        /// <summary>
        /// Represents "gl_NextBuffer" when specifying feedback varyings and using advanced interleaving.
        /// </summary>
        public static TransformOut NextBuffer = new TransformOut("gl_NextBuffer");

        /// <summary>
        /// Represents "gl_SkipComponents1" when specifying feedback varyings and using advanced interleaving.
        /// </summary>
        public static TransformOut SkipComponents1 = new TransformOut("gl_SkipComponents1");

        /// <summary>
        /// Represents "gl_SkipComponents2" when specifying feedback varyings and using advanced interleaving.
        /// </summary>
        public static TransformOut SkipComponents2 = new TransformOut("gl_SkipComponents2");

        /// <summary>
        /// Represents "gl_SkipComponents3" when specifying feedback varyings and using advanced interleaving.
        /// </summary>
        public static TransformOut SkipComponents3 = new TransformOut("gl_SkipComponents3");

        /// <summary>
        /// Represents "gl_SkipComponents4" when specifying feedback varyings and using advanced interleaving.
        /// </summary>
        public static TransformOut SkipComponents4 = new TransformOut("gl_SkipComponents4");

        /// <summary>
        /// Holds a list of all special keywords which can only be used with advanced interleaving.
        /// </summary>
        private static readonly TransformOut[] SpecialOuts = { NextBuffer, SkipComponents1, SkipComponents2, SkipComponents3, SkipComponents4 };

        /// <summary>
        /// Specify values to record in transform feedback buffers.
        /// </summary>
        /// <remarks>
        /// Transform feedback varyings must be specified before linking the program. Either specify them in the constructor of the program
        /// or call <see cref="Program.Link"/> again after a call to this method.<br/>
        /// To specify the keywords introduced with advanced interleaving "gl_NextBuffer" and "gl_SkipComponents#"
        /// use the TransformOut dummy-instances <see cref="NextBuffer"/> and <see cref="SkipComponents1"/>, etc.
        /// </remarks>
        /// <param name="bufferMode">Identifies the mode used to capture the varying variables when transform feedback is active. bufferMode must be InterleavedAttribs or SeparateAttribs.</param>
        /// <param name="feedbackVaryings">An array of TransformOut objects specifying the varying variables to use for transform feedback.</param>
        public void FeedbackVaryings(TransformFeedbackMode bufferMode, params TransformOut[] feedbackVaryings)
        {
            //TODO: find out if the varyings buffer binding indices can be queried from OpenGL
            var index = 0;
            foreach (var output in feedbackVaryings)
            {
                if (bufferMode == TransformFeedbackMode.SeparateAttribs && SpecialOuts.Contains(output))
                    throw new DerpGLException("Advanced interleaving features can not be used with separate mode.");
                // set the outputs buffer binding index
                output.Index = index;
                // increase the index if all outputs get routed to a separate bindind or if the gl_NextBuffer keyword is found
                if (bufferMode == TransformFeedbackMode.SeparateAttribs || output == NextBuffer) index++;
            }
            GL.TransformFeedbackVaryings(Handle, feedbackVaryings.Length, feedbackVaryings.Select(_ => _.Name).ToArray(), bufferMode);
        }
    }
}