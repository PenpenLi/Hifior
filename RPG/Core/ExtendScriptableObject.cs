using UnityEngine;

public class ExtendScriptableObject : ScriptableObject
{
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
