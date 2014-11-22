﻿using System;

namespace Nelibur.Sword.Logging
{
    internal interface ILog
    {
        bool IsDebugEnabled { get; }

        void Debug(object message);
        void Debug(object message, Exception exception);
        void DebugFormat(string format, params object[] args);

        void Error(object message);
        void Error(object message, Exception exception);

        void ErrorFormat(string format, params object[] args);
        void Fatal(object message);
        void Fatal(object message, Exception exception);

        void Info(object message);
        void Info(object message, Exception exception);
        void InfoFormat(string format, params object[] args);

        void Warn(object message);
        void Warn(object message, Exception exception);
        void WarnFormat(string format, params object[] args);
    }
}
