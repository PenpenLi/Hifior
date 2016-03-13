using UnityEngine;
using System;
using System.IO;
using UnityEngine.Assertions;
/// <summary>
/// 可序列化物体的基类，如果你需要存储数据，新建一个类继承该类，可以使用List记录数组数据，List中的类也要是[Serializable]注明的
/// </summary>
[Serializable]
public abstract class SerializableBase : ISaveLoad
{
    public string Key;
    public string Now;
    public SerializableBase()
    {
    }

    public abstract string GetFullRecordPathName();// { return ""; }
    public abstract string GetKey();// { return ""; }

    public T LoadFromDisk<T>()
    {
        FileInfo t = new FileInfo(GetFullRecordPathName());
        Assert.IsTrue(t.Exists, "无法读取到磁盘文件");
        StreamReader sr = new StreamReader(GetFullRecordPathName());
        string s = sr.ReadToEnd();
        return JsonUtility.FromJson<T>(s);
    }

    public void SaveToDisk()
    {
        Key = GetKey();
        Now = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        string s = JsonUtility.ToJson(this);

        FileInfo t = new FileInfo(GetFullRecordPathName());
        if (t.Exists) //如果文件存在的话，清空里面的内容先
        {
            FileStream stream = File.Open(GetFullRecordPathName(), FileMode.OpenOrCreate);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();
        }
        StreamWriter sw;
        sw = t.AppendText();//以追加的方式打开

        sw.Write(s);
        sw.Close();
    }
}
