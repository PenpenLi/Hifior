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
#if UNITY_EDITOR
    [ContextMenu("Create AssetBundles")]
    public void CreateAssetBundleThemelves()
    {
        string targetPath = Application.dataPath + "/StreamingAssets";
        if (BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.ChunkBasedCompression))
        {

            Debug.Log("packed successfully!");
        }
        else {
            Debug.Log("packed failly!");
        }
        AssetDatabase.Refresh();
    }
    [ContextMenu("Get AssetBundle names")]
    public void GetNames()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log("AssetBundle: " + name);
    }
#endif
}
