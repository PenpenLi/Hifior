using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class GameObjectMenuItem : Editor
{
    [MenuItem("GameObject/Misc/ShowPath")]
    static void ShowHierarchyItemPath()
    {
        GameObject[] o = Selection.gameObjects;
        if (o.Length == 1)
        {
            GameObject obj = o[0];
            string path = obj.name;
            while (obj.transform.parent != null)
            {
                path = string.Format("{0}/{1}", obj.transform.parent.name, path);
                obj = obj.transform.parent.gameObject;
            }
            Debug.Log(path);
        }
    }

}
