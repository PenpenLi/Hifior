using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public abstract class UDebugCommand : ICommand
{
    protected string[] paramArray;
    public abstract void Do();
    public UDebugCommand(params string[] paramenters)
    {
        paramArray = paramenters;
    }
}

[DebugCommand("Test")]
public class TestDebugCommand : UDebugCommand
{
    public override void Do()
    {
        Log.Write<string>(paramArray);
    }
    public TestDebugCommand(params string[] paramenters) : base(paramenters) { }
}

public class UDebugCommandManager
{
    //[RuntimeInitializeOnLoadMethod]
    //static void Run()
    //{
    //    Start();
    //    Exec("xxx ... 66");
    //}
    static Dictionary<string, Type> _handles = new Dictionary<string, Type>();
    [Conditional("UNITY_EDITOR"), Conditional("UNITY_STANDALONE_WIN"), Conditional("FORCE_LOG"), Conditional("UNITY_ANDROID")]
    public static void Start()
    {
        // _handles.Add(".modifyproperty", new ModifyPropertyCommandHandle());
        ClassEnumerator enumerator = new ClassEnumerator(typeof(DebugCommandAttribute), null, typeof(UDebugCommand).Assembly, true, false, false);
        ListView<System.Type>.Enumerator enumerator2 = enumerator.results.GetEnumerator();
        while (enumerator2.MoveNext())
        {
            Type t = enumerator2.Current;
            if (!t.IsSubclassOf(typeof(UDebugCommand)))
            {
                Log.Write("类型：" + t.ToString() + "不是UDebugCommand的子类，请不要添加DebugCommandAttribute");
            }
            object[] customAttributes = t.GetCustomAttributes(typeof(DebugCommandAttribute), false);
            if (customAttributes.Length > 0)
            {
                DebugCommandAttribute attribute = customAttributes[0] as DebugCommandAttribute;
                if (attribute != null)
                {
                    _handles.Add(attribute.command, t);
                }
            }
        }
    }

    public static void End()
    {
        _handles.Clear();
    }

    public static string GetParamString(string[] paramList)
    {
        string paramString = "";
        for (Int32 i = 0; i < paramList.Length; ++i)
        {
            if (i == 0)
            {
                continue;
            }

            paramString += (" " + paramList[i]);
        }
        return paramString;
    }

    public static void Parse(string command, ref string func, ref string[] paramList)
    {
        string[] list = command.Split(' ');
        if (list.Length <= 0)
        {
            return;
        }
        func = list[0];
        paramList = new string[list.Length - 1];
        Array.Copy(list, 1, paramList, 0, list.Length - 1);
    }

    public static bool Exec(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            return false;
        }
        //
        string func = string.Empty;
        string[] paramList = null;
        Parse(command, ref func, ref paramList);

        UDebugCommand handle = GetCommandHandle(func.ToLower(), paramList);
        if (handle != null)
        {
            handle.Do();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static UDebugCommand GetCommandHandle(string command, string[] paramList)
    {
        Type t = null;
        _handles.TryGetValue(command, out t);
        UDebugCommand udc = (UDebugCommand)Activator.CreateInstance(t, new object[] { paramList });
        return udc;
    }
}
