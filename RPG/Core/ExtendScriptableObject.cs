using UnityEngine;

public class ExtendScriptableObject : ScriptableObject
{
    [ContextMenu("Json")]
    public void ShowJson()
    {
        Debug.Log(ToJson());
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
