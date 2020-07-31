//
// LogFactory.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using System.IO;
using log4net;

namespace ObjectTK.Logging
{
    public static class LogFactory
    {
        public static readonly bool IsAvailable = File.Exists(AppDomain.CurrentDomain.BaseDirectory + "log4net.dll");

        static IObjectTKLogger CreateLogger(Type type)
        {
            var logger = LogManager.GetLogger(type);
            return logger != null ? new DefaultLogImpl(logger) : null;
        }

        public static IObjectTKLogger GetLogger(Type type)
        {
            return IsAvailable ? CreateLogger(type) : null;
        }
    }
}
