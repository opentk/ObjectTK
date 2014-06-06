using System;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace DerpGL
{
    public static class Utility
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Utility));

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
            Logger.Error(string.Format("Assert failed: {0}\n{1}", value, errorMessage));
            throw new ApplicationException(string.Format("ErrorCode: {0}\n{1}", value, errorMessage));
        }
    }
}
