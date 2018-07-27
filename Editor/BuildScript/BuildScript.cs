using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BuildScript
{
    public static string AssetBundlesOutputPath
    {
        get
        {
            return "Assets/StreamingAssets";
        }
    }



    /// <summary>
    /// 批量打包选中的资源（包括子节点）
    /// </summary>
    /// <param name="outputPath">导出的路径，比如 Assetbundles/Scene</param>
    public static void BuildSelectedAssetBundles(string outputPath = "Assetbundles")
    {
        CreateAssetBundleForChildrenPrefab();
        CreateAssetBundleForChildrenMaterial();
        CreateAssetBundleForChildrenTexture();
        List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            if (!ValidateResource(obj))
            {
                continue;
            }
            string assetPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
            AssetBundleBuild build = new AssetBundleBuild();
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            build.assetBundleName = importer.assetBundleName;
            build.assetBundleVariant = importer.assetBundleVariant;
            build.assetNames = new string[] { assetPath };
            buildList.Add(build);
        }
        BuildPipeline.BuildAssetBundles(outputPath, buildList.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

    }

    public static bool ValidateResource(Object obj)
    {
        if (obj == null)
        {
            return false;
        }
        PrefabType prefabType = PrefabUtility.GetPrefabType(obj);

        if (prefabType != PrefabType.Prefab && !(obj is Texture) && !(obj is Material))
        {
            return false;
        }
        return true;
    }

    public static void CreateAssetBundle()
    {
        foreach (UnityEngine.Object iterObject in Selection.objects)
        {
            BuildAssisstant.CreateAssetBundle(iterObject);
        }
    }

    public static void BuildAssetBundles()
    {
        if (BuildPipeline.BuildAssetBundles(AssetBundlesOutputPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64))
        {
            Debug.Log("packed successfully!");
        }
        else
        {
            Debug.Log("packed failly!");
        }
        AssetDatabase.Refresh();
    }

    public static void ClearAssetBundle()
    {
        foreach (UnityEngine.Object iterObject in Selection.objects)
        {
            BuildAssisstant.ClearAssetBundle(iterObject);
        }
    }

    public static void ClearAssetBundleForChildren()
    {
        string path = UnityEditor.AssetDatabase.GetAssetPath(Selection.activeObject);
        List<UnityEngine.Object> objList = BuildAssisstant.CollectAll<UnityEngine.Object>(path);
        foreach (UnityEngine.Object iterObject in objList)
        {
            BuildAssisstant.ClearAssetBundle(iterObject);
        }
    }

    public static void CreateAssetBundleForChildren<T>(string append)
        where T : UnityEngine.Object
    {
        string path = UnityEditor.AssetDatabase.GetAssetPath(Selection.activeObject);
        List<T> objList = BuildAssisstant.CollectAll<T>(path);
        foreach (UnityEngine.Object iterObject in objList)
        {
            BuildAssisstant.CreateAssetBundle(iterObject, append);
        }
    }

    public static void CreateAssetBundleForChildrenPrefab()
    {
        //UnityEngine.Object[] prefabs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        //foreach (UnityEngine.Object iterObject in prefabs)
        //{
        //    PrefabType prefabType = PrefabUtility.GetPrefabType(iterObject);
        //    if (prefabType != PrefabType.Prefab)
        //    {
        //        continue;
        //    }
        //    //
        //    BuildAssisstant.CreateAssetBundle(iterObject);
        //}
        string path = UnityEditor.AssetDatabase.GetAssetPath(Selection.activeObject);
        List<GameObject> objList = BuildAssisstant.CollectAll<GameObject>(path);
        foreach (UnityEngine.Object iterObject in objList)
        {
            PrefabType prefabType = PrefabUtility.GetPrefabType(iterObject);
            if (prefabType != PrefabType.Prefab)
            {
                continue;
            }
            BuildAssisstant.CreateAssetBundle(iterObject);
        }
    }

    public static void CreateAssetBundleForChildrenMaterial()
    {
        CreateAssetBundleForChildren<Material>("_material");
    }

    public static void CreateAssetBundleForChildrenTexture()
    {
        CreateAssetBundleForChildren<Texture>("_texture");
    }

}
