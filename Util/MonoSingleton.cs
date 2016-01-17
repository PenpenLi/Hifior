using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 继承该类的脚本只应当在一个场景中出现一个实例,该物体也不会随着场景的载入而销毁
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : class
{
    public static T Instance
    {
        get;
        private set;
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this as T;
    }
    public MonoSingleton()
    {
        Instance = this as T;
    }
}
/// <summary>
/// 普通类的单例 确保不会重复调用,所有使用单例的类都应当手动进行初始化
/// </summary>
public class Singleton
{
    static Singleton instance;

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }
}
