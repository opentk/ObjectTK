//
// Utility.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using ObjectTK.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK
{
    internal static class Utility
    {
        private static readonly Logging.IObjectTKLogger Logger = Logging.LogFactory.GetLogger(typeof(Utility));

        public static void Assert(string errorMessage)
        {
            Assert(GL.GetError(), ErrorCode.NoError, errorMessage);
        }

        public static void Assert(ErrorCode desiredErrorCode, string errorMessage)
        {
            Assert(GL.GetError(), desiredErrorCode, errorMessage);
        }

        public static void Assert<T>(T value, T desiredValue, string errorMessage)
        {
            if (desiredValue.Equals(value)) return;
            Logger?.Error(string.Format("Assert failed: {0}\n{1}", value, errorMessage));
            throw new ObjectTKException(string.Format("ErrorCode: {0}\n{1}", value, errorMessage));
        }
    }
}
