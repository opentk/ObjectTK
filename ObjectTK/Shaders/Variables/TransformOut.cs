//
// TransformOut.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Represents a transform feedback output varying.
    /// </summary>
    public sealed class TransformOut
        : ProgramVariable
    {
        /// <summary>
        /// Specifies the transform feedback buffer binding index of this output.
        /// </summary>
        public int Index { get; internal set; }

        internal TransformOut()
        {
            Index = -1;
            Active = false;
        }

        /// <summary>
        /// Initializes a dummy instance of TransformOut used for the keywords introduced with advanced interleaving.
        /// </summary>
        internal TransformOut(string name)
        {
            Name = name;
        }
    }
}