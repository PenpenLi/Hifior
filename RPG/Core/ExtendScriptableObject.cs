#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ExtendScriptableObject : ScriptableObject
{
    [ContextMenu("Show Json")]
    public void ShowJson()
    {
        Debug.Log(ToJson());
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
