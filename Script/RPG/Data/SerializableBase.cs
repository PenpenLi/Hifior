using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Assertions;
/// <summary>
/// 可序列化物体的基类，如果你需要存储数据，新建一个类继承该类，可以使用List记录数组数据，List中的类也要是[Serializable]注明的
/// </summary>
[Serializable]
public abstract class SerializableBase
{
    public string Key;
    public string Now;
    /// <summary>
    /// 获取存储的路径，重写该函数后可以使用序列化
    /// </summary>
    /// <returns></returns>
    public virtual string GetFullRecordPathName() { return Application.persistentDataPath + "/save.json"; }
    /// <summary>
    /// 获取Key，重写以实现序列化操作
    /// </summary>
    /// <returns></returns>
    public abstract string GetKey();
    public void SaveBinary()
    {
        Key = GetKey();
        RefreshTime();
        Assert.IsTrue(GetFullRecordPathName().Length > 0 || GetKey().Length > 0, "请重写 GetFullRecordPathName()和 GetKey()方法");
        Now = Utils.TextUtil.GetStandardDataTime();

        BinaryFormatter binary = new BinaryFormatter();
        FileStream fStream = File.Create(GetFullRecordPathName());

        binary.Serialize(fStream, this);
        fStream.Close();
    }

    public T LoadBinary<T>() where T : SerializableBase
    {
        Assert.IsTrue(Exists(), "无法读取到磁盘文件");
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream fStream = File.Open(GetFullRecordPathName(), FileMode.Open);
            T ret = binary.Deserialize(fStream) as T;
            fStream.Close();
            return ret;
        }
    }

    public bool Exists()
    {
        return File.Exists(GetFullRecordPathName());
    }

    public T Load<T>() where T : SerializableBase
    {
        Assert.IsTrue(Exists(), "无法读取到磁盘文件");
        StreamReader sr = new StreamReader(GetFullRecordPathName());
        string s = sr.ReadToEnd();
        sr.Close();
        return JsonUtility.FromJson<T>(s);
    }

    public void Save()
    {
        Key = GetKey();
        RefreshTime();
        string s = JsonUtility.ToJson(this);
        var fullPath = GetFullRecordPathName();
        var fullDir = Path.GetDirectoryName(fullPath);
        if (Directory.Exists(fullDir) == false)
        {
            Directory.CreateDirectory(fullDir);
        }
        FileInfo t = new FileInfo(fullPath);
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
    protected void RefreshTime()
    {
        Now = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    }
}
/*
[Serializable]
public class Save_Character : SerializableBase
{
    public int ID;
    public string Name;

    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/Character0.sav";
    }

    public override string GetKey()
    {
        return "Character0";
    }
    [RuntimeInitializeOnLoadMethod]
    public static void III()
    {
        bool save = false;
        if (save)
        {
            Save_Character s = new Save_Character();
            Debug.Log("RuntimeInit" + s.GetFullRecordPathName());
            s.ID = 0;
            s.Name = "是方法就是看";
            s.SaveBinary();
        }
        else
        {

            Save_Character s = new Save_Character();
            s = s.LoadBinary<Save_Character>();
            Debug.Log(s.ID + "  " + s.Name);
        }
    }
}
[Serializable]
public class Save_CharacterList : SerializableList<Save_Character>
{
    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/Characters.sav";
    }

    public override string GetKey()
    {
        return "Characters";
    }
    [RuntimeInitializeOnLoadMethod]
    public static void III()
    {
        bool save = false;
        if (save)
        {
            Save_CharacterList sss = new Save_CharacterList();
            sss.RecordList = new List<Save_Character>();

            Save_Character s0 = new Save_Character();
            s0.Key = "0";
            s0.ID = 0;
            s0.Name = "是方法就是看";
            Save_Character s1 = new Save_Character();
            s1.Key = "liu";
            s1.ID = 1;
            s1.Name = "fsdafasdfdsa";
            sss.RecordList.Add(s0);
            sss.RecordList.Add(s1);

            sss.SaveBinary();
        }
        else
        {
            Save_CharacterList sss = new Save_CharacterList();
            Debug.Log(sss.GetBinaryContent("liu").Name);
        }
    }
}
*/
