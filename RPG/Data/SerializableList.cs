using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Assertions;
using UnityEngine;
/// <summary>
/// 生命为抽象类，这样可以不用实现父类中的抽象方法
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public abstract class SerializableList<T> : SerializableBase where T : SerializableBase
{
    public List<T> RecordList;

    public void AddContent(T Content)
    {
        CheckRecordList();
        RecordList.Add(Content);
    }
    public void AddContent(List<T> Content)
    {
        CheckRecordList();
        RecordList.AddRange(Content);
    }
    private void CheckRecordList()
    {
        if (RecordList == null)
            RecordList = new List<T>();
        RefreshTime();
    }
    public T GetContent(string Key)
    {
        if (RecordList == null)
            RecordList = Load<SerializableList<T>>().RecordList;

        for (int i = 0; i < RecordList.Count; i++)
        {
            if (RecordList[i].Key.Equals(Key))
            {
                return RecordList[i] as T;
            }
        }
        return default(T);
    }

    public T GetBinaryContent(string Key)
    {
        if (RecordList == null)
            RecordList = LoadBinary<SerializableList<T>>().RecordList;

        for (int i = 0; i < RecordList.Count; i++)
        {
            if (RecordList[i].Key.Equals(Key))
            {
                return RecordList[i] as T;
            }
        }
        return default(T);
    }
}
