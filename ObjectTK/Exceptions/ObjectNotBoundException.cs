//
// ObjectNotBoundException.cs
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
    /// The exception that is thrown when an object is used which must be bound before usage.
    /// </summary>
    [Serializable]
    public class ObjectNotBoundException
        : ObjectTKException
    {
        internal ObjectNotBoundException(string message)
            : base(message)
        {
        }
    }
}