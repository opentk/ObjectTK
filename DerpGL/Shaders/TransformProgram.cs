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
        public static TransformOut NextBuffer = new TransformOut("gl_NextBuffer");
        public static TransformOut SkipComponents1 = new TransformOut("gl_SkipComponents1");
        public static TransformOut SkipComponents2 = new TransformOut("gl_SkipComponents2");
        public static TransformOut SkipComponents3 = new TransformOut("gl_SkipComponents3");
        public static TransformOut SkipComponents4 = new TransformOut("gl_SkipComponents4");
        private static readonly TransformOut[] SpecialOuts = { NextBuffer, SkipComponents1, SkipComponents2, SkipComponents3, SkipComponents4 };

        protected void FeedbackVaryings(TransformFeedbackMode bufferMode, params TransformOut[] feedbackVaryings)
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