using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid
{
    public static class Utility
    {
        public static void Assert(string errorMessage)
        {
            Assert(GL.GetError, ErrorCode.NoError, errorMessage);
        }

        public static void Assert(ErrorCode desiredErrorCode, string errorMessage)
        {
            Assert(GL.GetError, desiredErrorCode, errorMessage);
        }

        public static void Assert<T>(Func<T> method, T desiredErrorCode, string errorMessage)
        {
            var errorCode = method();
            if (!desiredErrorCode.Equals(errorCode))
            {
                throw new ApplicationException(string.Format("ErrorCode: {0}\n{1}", errorCode, errorMessage));
            }
        }
    }
}
