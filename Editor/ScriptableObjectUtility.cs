using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    /// <summary>
    //  更方便的建立 ScriptableObject asset 文件.
    /// </summary>
    public static T CreateAsset<T>(
          string tentativeName = null,
          string givenPath = null,
          bool select = true
      ) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = givenPath ??
                AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(
                path + "/" + (tentativeName ?? ("New " + typeof(T).ToString())) + ".asset"
            );
        // Debug.Log("given "+givenPath+" actual "+path+" tent "+tentativeName+" path "+assetPathAndName);

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        //Undo.RegisterCreatedObjectUndo(asset, "Create "+typeof(T).ToString());

        if (select)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
        return asset;
    }
    public static int GetFoldFileCount(string path)
    {
        return Directory.GetFiles(path).GetLength(0)/2;
    }
}
