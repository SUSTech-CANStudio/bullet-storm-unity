using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CANStudio.BulletStorm.Util
{
    public static class BulletStormLogger
    {
        private static readonly Lazy<Logger> Instance = new Lazy<Logger>(() => new Logger(new Handler()));
#if UNITY_EDITOR
        private static readonly HashSet<object> Logged = new HashSet<object>();
#endif

        private static Logger Logger => Instance.Value;

        public static bool LogEnabled
        {
            get => Logger.logEnabled;
            set => Logger.logEnabled = value;
        }

        public static void Log(object message, Object context)
        {
            Log(LogType.Log, message, context);
        }

        public static void LogWarning(object message)
        {
            Log(LogType.Warning, message);
        }

        public static void LogWarning(object message, Object context)
        {
            Log(LogType.Warning, message, context);
        }

        public static void LogError(object message, Object context)
        {
            Log(LogType.Error, message, context);
        }

        public static void LogOnce(object message, Object context)
        {
#if UNITY_EDITOR
            if (Logged.Add(message)) Log(LogType.Log, message, context);
#endif
        }

        public static void LogWarningOnce(object message, Object context)
        {
#if UNITY_EDITOR
            if (Logged.Add(message)) Log(LogType.Warning, message, context);
#endif
        }

        public static void LogErrorOnce(object message, Object context)
        {
#if UNITY_EDITOR
            if (Logged.Add(message)) Log(LogType.Error, message, context);
#endif
        }

        private static bool IsLogTypeAllowed(LogType logType)
        {
            return Logger.IsLogTypeAllowed(logType);
        }

        private static void Log(LogType logType, object message)
        {
            Logger.Log(logType, message);
        }

        private static void Log(LogType logType, object message, Object context)
        {
            Logger.Log(logType, message, context);
        }

        private static void Log(LogType logType, string tag, object message)
        {
            Logger.Log(logType, tag, message);
        }

        private static void Log(LogType logType, string tag, object message, Object context)
        {
            Logger.Log(logType, tag, message, context);
        }

        public static void Log(object message)
        {
            Logger.Log(message);
        }

        public static void LogException(Exception exception)
        {
            Logger.LogException(exception);
        }

        public static void LogException(Exception exception, Object context)
        {
            Logger.LogException(exception, context);
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
    }
}