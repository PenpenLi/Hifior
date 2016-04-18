using UnityEditor;
using UnityEngine;

namespace RPGEditor
{
    public class AssetBundleBuilder
    {
        [MenuItem("Window/Get Resources File Path")]
        public static void GetPath()
        {
            string s = AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets/Resources/", "");
            Debug.Log(s.Remove(s.LastIndexOf('.')));
        }
        [MenuItem("RPGEditor/Misc/Build Asset Bundle")]
        public static void CreateAssetBundleThemelves()
        {
            string targetPath = Application.dataPath + "/StreamingAssets";
            if (BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64))
            {

                Debug.Log("packed successfully!");
            }
            else
            {
                Debug.Log("packed failly!");
            }
            AssetDatabase.Refresh();
        }
        [MenuItem("RPGEditor/Misc/Get Asset Bundle Names")]
        public static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
                Debug.Log("AssetBundle: " + name);
        }
    }
}