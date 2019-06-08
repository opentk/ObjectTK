//
// ObjectTKException.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;

namespace ObjectTK.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an ObjectTK related error occurs.
    /// </summary>
    [Serializable]
    public class ObjectTKException
        : Exception
    {
        internal ObjectTKException(string message)
            : base(message)
        {
        }
    }
}