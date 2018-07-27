using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

#pragma warning disable 0414 // 已被赋值，但从未使用过它的值;

public class AssetbundlesMenuItems
{
	[MenuItem("Assets/BuildAssetbundlesForSelected")]
	public static void BuildAssetbundlesForSelected()
	{
		BuildScript.BuildSelectedAssetBundles();
	}
    
	[MenuItem("Assets/AssetBundle/Create")]
	static void CreateAssetBundle()
	{
		BuildScript.CreateAssetBundle();
	}

	[MenuItem("Assets/AssetBundle/Clear")]
	static void ClearAssetBundle()
	{
		BuildScript.ClearAssetBundle();
	}

	[MenuItem("Assets/AssetBundle/ClearChildren")]
	static void ClearAssetBundleForChildren()
	{
		BuildScript.ClearAssetBundleForChildren();
	}

	[MenuItem("Assets/AssetBundle/CreateChildren/Prefab")]
	public static void CreateAssetBundleForChildrenPrefab()
	{
		BuildScript.CreateAssetBundleForChildrenPrefab();
	}

	[MenuItem("Assets/AssetBundle/CreateChildren/Material")]
	public static void CreateAssetBundleForChildrenMaterial()
	{
		BuildScript.CreateAssetBundleForChildrenMaterial();
	}

	[MenuItem("Assets/AssetBundle/CreateChildren/Texture")]
	public static void CreateAssetBundleForChildrenTexture()
	{
		BuildScript.CreateAssetBundleForChildrenTexture();
	}

	[MenuItem("Assets/AssetBundle/CreateCharacter")]
	public static void CreateCharacter()
	{
		//BuildAvatar.Export();
	}

    [MenuItem("Assets/Log Resources Relative Path")]
    public static void GetResourcesRelativePath()
    {
        string s = AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets/Resources/", "");
        Debug.Log(s.Remove(s.LastIndexOf('.')));
    }
    [MenuItem("Assets/Log Assets Full Path")]
    public static void GetPath()
    {
        string s = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log(s);
    }
    [MenuItem("Export/Build Asset Bundle")]
    public static void CreateAssetBundleThemelves()
    {
        BuildScript.BuildAssetBundles();
    }

    [MenuItem("Export/Log All Asset Bundle Names")]
    public static void GetNames()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log("AssetBundle: " + name);
    }
}
