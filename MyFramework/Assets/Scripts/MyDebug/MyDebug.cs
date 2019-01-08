//#define DEBUG_LEVEL_NORMAL // 普通
//#define DEBUG_LEVEL_WARNING // 警告
//#define DEBUG_LEVEL_ERROR // 错误
//#define DEBUG_LEVEL_EXCEPTION // 异常
//#define DEBUG_LEVEL_ASSERT // 断言
// 日志开关
// DEBUG_LEVEL_NORMAL;DEBUG_LEVEL_WARNING;DEBUG_LEVEL_ERROR;DEBUG_LEVEL_EXCEPTION;DEBUG_LEVEL_ASSERT;
/* 
 注：SDDebug也可称为Debug管理.使用时，若不想在哪个平台看到打印日志，只需在PlayerSetting --> Ohter Settings -->scripting define symbols中不添加
    （DEBUG_LEVEL_NORMAL;DEBUG_LEVEL_WARNING;DEBUG_LEVEL_ERROR;DEBUG_LEVEL_EXCEPTION;DEBUG_LEVEL_ASSERT;）即可。若想使用，则需添加.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// Debug输出类
/// </summary>
public static class MyDebug
{
    private static List<string> _outPutLog = new List<string>();  //需写入本地的log
    private static List<string> _outPutLogBuffer = new List<string>();
    private static Thread _thread;
    private static string _logfilePath;
    public static void Init(string logPath)
    {
        _logfilePath = logPath;
        if (_thread == null)
        {
            _thread = new Thread(new ThreadStart(ThreadRecv));
            _thread.IsBackground = true;
            _thread.Start();
        }
    }

    public static void Clear()
    {
        _thread.Abort();
    }

    public static void AddOutPutLog(object message)
    {
        string log = FormatLog("异常：", message == null ? "null" : message.ToString());
        lock (_outPutLog)
        {
            _outPutLog.Add(log);
        }
    }

    /// <summary>
    /// 线程写入本地Log文件
    /// </summary>
    public static void ThreadRecv()
    {
        StreamWriter writer = new StreamWriter(_logfilePath, true, Encoding.UTF8);
        bool write = false;
        int step = 10;
        while (true)
        {
            Thread.Sleep(100);
            lock (_outPutLog)
            {
                _outPutLogBuffer.AddRange(_outPutLog);
                _outPutLog.Clear();
            }

            for (int i = 0; i < _outPutLogBuffer.Count; i++)
            {
                writer.WriteLine(_outPutLogBuffer[i]);
            }
            _outPutLogBuffer.Clear();
            step--;
            if (step <= 0)
            {
                step = 10;
            }

            if (write != null)
            {
                writer.Flush();
            }
        }
    }

    

    [System.Diagnostics.Conditional("DEBUG_LEVEL_NORMAL")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [System.Diagnostics.Conditional("DEBUG_LEVEL_NORMAL")]
    public static void LogFormat(string format, params object[] args)
    {
        Debug.LogFormat(format, args);
    }

    [System.Diagnostics.Conditional("DEBUG_LEVEL_WARNING")]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    [System.Diagnostics.Conditional("DEBUG_LEVEL_WARNING")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        Debug.LogWarningFormat(format, args);
    }

    [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
    public static void LogError(object message)
    {
        string log = FormatLog("Error", message == null ? "null" : message.ToString());
        lock (_outPutLog)
        {
            _outPutLog.Add(log);
        }
        Debug.LogError(message);
    }

    [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        string log = FormatLog("Error", string.Format(format, args));
        lock (_outPutLog)
        {
            _outPutLog.Add(log);
        }
        Debug.LogErrorFormat(format, args);
    }

    private static string FormatLog(string type, string content)
    {
        return string.Format("MyDebugInfos##{2}:{0}::{1}", DateTime.Now, content, type);
    }

    private enum ELogType
    {
        Debug,
        Warning,
        Error,
    }
}
