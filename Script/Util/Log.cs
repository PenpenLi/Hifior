using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.Internal;

/// <summary>
/// 有必要输出的Log日志通过此函数打印，非必要的自己调试使用Write,提交需删除
/// </summary>
public static class Log
{
    public static void Write(object obj, LogType type = LogType.Log)
    {
        if (obj == null)
        {
            Debug.LogError("试图打印的对象为Null");
        }
        switch (type)
        {
            case LogType.Warning:
                Debug.LogWarning(obj.ToString());
                break;
            case LogType.Error:
                Debug.LogError(obj.ToString());
                break;
            default:
                Debug.Log(obj.ToString());
                break;
        }
    }
    public static void Write([DefaultValue("LogType.Log")] LogType type = LogType.Log, params object[] objs)
    {
        if (objs.Length < 1)
            Write("数组为0，无数据显示");
        else
        {
            string log = null;
            for (int i = 0; i < objs.Length - 1; i++)
            {
                log += objs[i].ToString() + '\n';
            }
            log += objs[objs.Length - 1].ToString();
            Write(log);
        }
    }
    public static void Write<T>(List<T> obj, LogType type = LogType.Log)
    {
        foreach (T item in obj)
        {
            Write(item.ToString() + "\n");
        }
    }
    public static void Write<T>(T[] obj, LogType type = LogType.Log)
    {
        foreach (T item in obj)
        {
            Write(item.ToString() + "\n");
        }
    }
    public static void Write<T1, T2>(Dictionary<T1, T2> obj, LogType type = LogType.Log)
    {
        foreach (KeyValuePair<T1, T2> item in obj)
        {
            Write("Key:" + item.Key + "  Value" + item.Value + "\n");
        }
    }

    [Conditional("UNITY_EDITOR")]
    public static void EditorWrit(object obj, LogType type = LogType.Log)
    {
        Write(obj, type);
    }

    [Conditional("UNITY_EDITOR")]
    public static void EditorWrite([DefaultValue("LogType.Log")] LogType type = LogType.Log, params object[] objs)
    {
        Write(type, objs);
    }
    [Conditional("UNITY_EDITOR")]
    public static void EditorWrite<T>(List<T> obj, LogType type = LogType.Log)
    {
        Write(obj, type);
    }
    [Conditional("UNITY_EDITOR")]
    public static void EditorWrite<T1, T2>(Dictionary<T1, T2> obj, LogType type = LogType.Log)
    {
        Write(obj, type);
    }
}