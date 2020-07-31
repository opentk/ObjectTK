//
// IObjectTKLogger.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectTK.Logging
{
    public interface IObjectTKLogger
    {
        bool IsFatalEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }

        void Debug(object message, Exception exception = null);
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        void DebugFormat(string format, params object[] args);

        void Error(object message, Exception exception = null);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);
        void ErrorFormat(string format, params object[] args);

        void Fatal(object message, Exception exception = null);
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
        void FatalFormat(string format, params object[] args);

        void Info(object message, Exception exception = null);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);
        void InfoFormat(string format, params object[] args);

        void Warn(object message, Exception exception = null);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
        void WarnFormat(string format, params object[] args);
    }
}
