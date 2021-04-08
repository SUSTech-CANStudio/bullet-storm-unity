using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CANStudio.BulletStorm.Util
{
    public static class BulletStormLogger
    {
        private static readonly Lazy<Logger> Instance = new Lazy<Logger>(() => new Logger(new Handler()));
        private static readonly HashSet<object> Logged = new HashSet<object>();

        private static Logger Logger => Instance.Value;

        public static void LogWarning(object message)
        {
            Log(LogType.Warning, message);
        }

        public static void LogError(object message)
        {
            Log(LogType.Error, message);
        }

        public static void LogOnce(object message)
        {
            if (Logged.Add(message)) Log(message);
        }

        public static void LogWarningOnce(object message)
        {
            if (Logged.Add(message)) LogWarning(message);
        }

        public static void LogErrorOnce(object message)
        {
            if (Logged.Add(message)) LogError(message);
        }

        private class Handler : ILogHandler
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            public void LogFormat(LogType logType, Object context, string format, params object[] args)
            {
                switch (logType)
                {
                    case LogType.Error:
                        format = "<color=red>BulletStorm: </color>" + format;
                        break;
                    case LogType.Assert:
                        format = "<color=olive>BulletStorm: </color>" + format;
                        break;
                    case LogType.Warning:
                        format = "<color=orange>BulletStorm: </color>" + format;
                        break;
                    case LogType.Log:
                        format = "<color=green>BulletStorm: </color>" + format;
                        break;
                    case LogType.Exception:
                        format = "<color=red>BulletStorm Exception: </color>" + format;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
                }

                format += "\n";
                Debug.unityLogger.logHandler.LogFormat(logType, context, format, args);
            }

            public void LogException(Exception exception, Object context)
            {
                Debug.unityLogger.logHandler.LogException(exception, context);
            }
        }

        #region generatedDelegates

        public static bool IsLogTypeAllowed(LogType logType)
        {
            return Logger.IsLogTypeAllowed(logType);
        }

        public static void Log(LogType logType, object message)
        {
            Logger.Log(logType, message);
        }

        public static void Log(LogType logType, object message, Object context)
        {
            Logger.Log(logType, message, context);
        }

        public static void Log(LogType logType, string tag, object message)
        {
            Logger.Log(logType, tag, message);
        }

        public static void Log(LogType logType, string tag, object message, Object context)
        {
            Logger.Log(logType, tag, message, context);
        }

        public static void Log(object message)
        {
            Logger.Log(message);
        }

        public static void Log(string tag, object message)
        {
            Logger.Log(tag, message);
        }

        public static void Log(string tag, object message, Object context)
        {
            Logger.Log(tag, message, context);
        }

        public static void LogWarning(string tag, object message)
        {
            Logger.LogWarning(tag, message);
        }

        public static void LogWarning(string tag, object message, Object context)
        {
            Logger.LogWarning(tag, message, context);
        }

        public static void LogError(string tag, object message)
        {
            Logger.LogError(tag, message);
        }

        public static void LogError(string tag, object message, Object context)
        {
            Logger.LogError(tag, message, context);
        }

        public static void LogFormat(LogType logType, string format, params object[] args)
        {
            Logger.LogFormat(logType, format, args);
        }

        public static void LogException(Exception exception)
        {
            Logger.LogException(exception);
        }

        public static void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            Logger.LogFormat(logType, context, format, args);
        }

        public static void LogException(Exception exception, Object context)
        {
            Logger.LogException(exception, context);
        }

        public static ILogHandler LogHandler
        {
            get => Logger.logHandler;
            set => Logger.logHandler = value;
        }

        public static bool LogEnabled
        {
            get => Logger.logEnabled;
            set => Logger.logEnabled = value;
        }

        public static LogType FilterLogType
        {
            get => Logger.filterLogType;
            set => Logger.filterLogType = value;
        }

        #endregion
    }
}