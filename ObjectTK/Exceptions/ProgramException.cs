//
// ProgramException.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Runtime.Serialization;

namespace ObjectTK.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a program related error occurs.
    /// </summary>
    [Serializable]
    public class ProgramException
        : ObjectTKException
    {
        public string InfoLog { get; private set; }

        internal ProgramException(string message, string infoLog)
            : base(message)
        {
            InfoLog = infoLog;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("InfoLog", InfoLog);
        }
    }
}