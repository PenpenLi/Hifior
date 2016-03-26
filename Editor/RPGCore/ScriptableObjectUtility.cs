using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

namespace RPGEditor
{
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
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
                return 0;
            }
            return Directory.GetFiles(path).GetLength(0) / 2;
        }
        /// <summary>
        /// 返回指定目录指定后缀的所有文件的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, params string[] postfix)
        {
            return Directory.GetFiles(path).Where(x =>
           {
               foreach (string s in postfix)
               {
                   if (x.EndsWith("." + s, StringComparison.OrdinalIgnoreCase))
                       return true;
               }
               return false;
           }).ToArray(); ;
        }
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
        public static string GetFileNameWithoutExtensionFromPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetFileNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }
    }
}