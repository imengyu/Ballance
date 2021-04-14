using System.Collections.Generic;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Log.cs
* 
* 用途：
* 基础日志静态类
* 
* Log.V(tag, message, ...) 打印一些最为繁琐、意义不大的日志信息 
* Log.D(tag, message, ...) 打印一些调试信息
* Log.I(tag, message, ...) 打印一些比较重要的数据，可帮助你分析用户行为数据
* Log.W(tag, message, ...) 打印一些警告信息
* Log.E(tag, message, ...) 印程序中的错误信息
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-14 创建
* 2021-4-12 imengyu 增加日志暂存区
*
*/

namespace Ballance2.Utils
{
    /// <summary>
    /// 日志器
    /// </summary>
    [SLua.CustomLuaClass]
    public static class Log
    {
        private static string TAG = "Log";

        public static void V(string tag, string message)
        {
            LogInternal(LogLevel.Verbose, tag, message);
        }
        public static void V(string tag, string format, params object[] param)
        {
            V(tag, string.Format(format, param));
        }
        public static void D(string tag, string message)
        {
            LogInternal(LogLevel.Debug, tag, message);
        }
        public static void D(string tag, string format, params object[] param)
        {
            D(tag, string.Format(format, param));
        }
        public static void I(string tag, string message)
        {
            LogInternal(LogLevel.Info, tag, message);
        }
        public static void I(string tag, string format, params object[] param)
        {
            I(tag, string.Format(format, param));
        }
        public static void W(string tag, string message)
        {
            LogInternal(LogLevel.Warning, tag, message);
        }
        public static void W(string tag, string format, params object[] param)
        {
            W(tag, string.Format(format, param));
        }
        public static void E(string tag, string message)
        {
            LogInternal(LogLevel.Error, tag, message);
        }
        public static void E(string tag, string format, params object[] param)
        {
            E(tag, string.Format(format, param));
        }

        private static void LogInternal(LogLevel level, string tag, string message) 
        {
            LogWrite(level, tag, message, DebugUtils.GetStackTrace(2));
        }

        /// <summary>
        /// 手动写入日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="tag">标签</param>
        /// <param name="message">信息</param>
        /// <param name="stackTrace">堆栈信息</param>
        public static void LogWrite(LogLevel level, string tag, string message, string stackTrace)
        {
            if(!logTemporaryForeachLock) {
                LogTemporaryData data = new LogTemporaryData();
                data.level = level;
                data.message = message;
                data.tag = tag;
                data.stackTrace = stackTrace;
                logTemporary.Add(data);
            }

            if(logTemporary.Count > 256)
                logTemporary.RemoveAt(0);

            observers.ForEach((observer) => {
                if ((observer.AcceptLevel & level) != LogLevel.None)
                    observer.Observer(level, tag, message, stackTrace);
            });
        }

        /// <summary>
        /// 重新发送暂存区中的日志条目
        /// </summary>
        public static void SendLogsInTemporary() {
            logTemporaryForeachLock = true;
            logTemporary.ForEach((data) => {
                observers.ForEach((observer) => {
                if ((observer.AcceptLevel & data.level) != LogLevel.None)
                    observer.Observer(data.level, data.tag, data.message, data.stackTrace);
                });
            });
            logTemporary.Clear();
            logTemporaryForeachLock = false;
        }

        /// <summary>
        /// 注册日志观察者
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="acceptLevel"></param>
        /// <returns>返回大于0的数字表示观察者ID，返回-1表示错误</returns>
        public static int RegisterLogObserver(LogObserver observer, LogLevel acceptLevel) 
        {
            if (acceptLevel == LogLevel.None)
            {
                E(TAG, "At least one LogLevel is required for LogObserver ! ");
                return -1;
            }

            LogObserverInternal logObserverInternal = observers.Find((o) => o.Observer == observer);
            if (logObserverInternal != null)
            {
                E(TAG, "Can not un register LogObserver {0} because it already registered! ", observer.GetHashCode());
                return -1;
            }

            logObserverInternal = new LogObserverInternal();
            logObserverInternal.Id = CommonUtils.GenAutoIncrementID();
            logObserverInternal.AcceptLevel = acceptLevel;
            logObserverInternal.Observer = observer;

            observers.Add(logObserverInternal);
            return logObserverInternal.Id;
        }
        /// <summary>
        /// 取消注册日志观察者
        /// </summary>
        /// <param name="id">观察者ID（由 RegisterLogObserver 返回）</param>
        public static void UnRegisterLogObserver(int id) 
        {
            LogObserverInternal logObserverInternal = observers.Find((o) => o.Id == id);
            if (logObserverInternal == null)
            {
                E(TAG, "Can not un register LogObserver {0} because it not registered! ", id);
                return;
            }
            observers.Remove(logObserverInternal);
        }
        /// <summary>
        /// 获取日志观察者
        /// </summary>
        /// <param name="id">观察者ID（由 RegisterLogObserver 返回）</param>
        /// <returns>如果找到则返回观察者，如果找不到则返回null</returns>
        public static LogObserver GetLogObserver(int id)
        {
            LogObserverInternal logObserverInternal = observers.Find((o) => o.Id == id);
            if (logObserverInternal != null)
                return logObserverInternal.Observer;
            return null;
        }

        public static string LogLevelToString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.None: return "N";
                case LogLevel.Verbose: return "V";
                case LogLevel.Debug: return "D";
                case LogLevel.Info: return "I";
                case LogLevel.Warning: return "W";
                case LogLevel.Error: return "E";
                case LogLevel.All: return "A";
            }
            return logLevel.ToString();
        }

        private struct LogTemporaryData {
            public LogLevel level;
            public string tag;
            public string message;
            public string stackTrace;
        }

        private static List<LogObserverInternal> observers = new List<LogObserverInternal>();
        private static List<LogTemporaryData> logTemporary = new List<LogTemporaryData>();
        private static bool logTemporaryForeachLock = false;

        /// <summary>
        /// 内部观察者保存类
        /// </summary>
        private class LogObserverInternal
        {
            public int Id;
            public LogObserver Observer;
            public LogLevel AcceptLevel;
        }
    }

    [SLua.CustomLuaClass]
    public enum LogLevel
    {
        None = 0,
        Verbose = 0x1,
        Debug = 0x2,
        Info = 0x4,
        Warning = 0x8,
        Error = 0x10,
        All = Verbose | Debug | Info | Warning | Error,
    }

    [SLua.CustomLuaClass]
    public delegate void LogObserver(LogLevel level, string tag, string message, string stackTrace);
}
