//
// DefaultLogImpl.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using log4net;
using log4net.Core;

namespace ObjectTK.Logging
{
    public class DefaultLogImpl : IObjectTKLogger
    {
        protected virtual Type ThisDeclaringType => typeof(DefaultLogImpl);
        protected ILogger _logger => _log.Logger;

        protected readonly ILog _log;

        public DefaultLogImpl(ILog logger)
        {
            _log = logger;
        }

        public bool IsFatalEnabled => _log.IsFatalEnabled;

        public bool IsWarnEnabled => _log.IsWarnEnabled;

        public bool IsInfoEnabled => _log.IsInfoEnabled;

        public bool IsDebugEnabled => _log.IsDebugEnabled;

        public bool IsErrorEnabled => _log.IsErrorEnabled;

        private void Log(Level level, object message, Exception exception = null)
        {
            if (!_logger.IsEnabledFor(level)) return;
            _logger.Log(ThisDeclaringType, level, message, exception);
        }

        private void LogFormat(Level level, IFormatProvider provider, string format, params object[] args)
        {
            if (!_logger.IsEnabledFor(level)) return;

            var message = (provider == null) ?
                string.Format(format, args) :
                string.Format(provider, format, args);

            _logger.Log(ThisDeclaringType, level, message, null);
        }

        private void LogFormat(Level level, string format, params object[] args)
        {
            LogFormat(null, format, args);
        }

        public void Debug(object message, Exception exception = null)
        {
            Log(Level.Debug, message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            LogFormat(Level.Debug, provider, format, args);
        }

        public void DebugFormat(string format, params object[] args)
        {
            LogFormat(Level.Debug, null, format, args);
        }

        public void Error(object message, Exception exception = null)
        {
            Log(Level.Error, message);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            LogFormat(Level.Error, provider, format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            LogFormat(Level.Error, null, format, args);
        }

        public void Fatal(object message, Exception exception = null)
        {
            Log(Level.Fatal, message);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            LogFormat(Level.Fatal, provider, format, args);
        }

        public void FatalFormat(string format, params object[] args)
        {
            LogFormat(Level.Fatal, null, format, args);
        }

        public void Info(object message, Exception exception = null)
        {
            Log(Level.Info, message);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            LogFormat(Level.Info, provider, format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            LogFormat(Level.Info, null, format, args);
        }

        public void Warn(object message, Exception exception = null)
        {
            Log(Level.Warn, message);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            LogFormat(Level.Warn, provider, format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            LogFormat(Level.Warn, null, format, args);
        }
    }
}
