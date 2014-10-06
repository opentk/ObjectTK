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
