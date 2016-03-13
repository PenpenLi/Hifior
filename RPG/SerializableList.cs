using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Assertions;
using UnityEngine;
[Serializable]
public abstract class SerializableList : SerializableBase
{
    public List<SerializableBase> RecordList;
    public T GetContent<T>(string Key) where T : SerializableBase
    {
        RecordList = LoadFromDisk<SerializableList>().RecordList;
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
