using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class BuildAssisstant
{
	static readonly string AssetPathHead = "Assets/";
	//
	public static UnityEngine.Object GetPrefab(UnityEngine.GameObject go, string path, string name)
	{
		string fullName = (path + "/" + name + ".prefab").ToLower();
		UnityEngine.Object tempPrefab = PrefabUtility.CreateEmptyPrefab(fullName);
		tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
		return tempPrefab;
	}

	public static string GetName(UnityEngine.Object obj)
	{
		string path = UnityEditor.AssetDatabase.GetAssetPath(obj);
		path = Path.GetDirectoryName(path);
		path = path.Replace(AssetPathHead, string.Empty);
		return (path + "/" + obj.name).ToLower();
	}

	public static void CreateAssetBundle(UnityEngine.Object obj)
	{
		string path = UnityEditor.AssetDatabase.GetAssetPath(obj);
		UnityEditor.AssetImporter importer = UnityEditor.AssetImporter.GetAtPath(path);
		if (importer != null)
		{
			importer.assetBundleName = GetName(obj);
			importer.SaveAndReimport();
		}
	}

	public static void CreateAssetBundle(UnityEngine.Object obj, string append)
	{
		string path = UnityEditor.AssetDatabase.GetAssetPath(obj);
		UnityEditor.AssetImporter importer = UnityEditor.AssetImporter.GetAtPath(path);
		if (importer != null)
		{
			importer.assetBundleName = GetName(obj) + append;
			importer.SaveAndReimport();
		}
	}

	public static void CreateAssetBundle<T>(List<T> objList, string append) where T : UnityEngine.Object
	{
		foreach (T iterObject in objList)
		{
			CreateAssetBundle(iterObject, append);
		}
	}

	public static void ClearAssetBundle(UnityEngine.Object obj)
	{
		string path = UnityEditor.AssetDatabase.GetAssetPath(obj);
		UnityEditor.AssetImporter importer = UnityEditor.AssetImporter.GetAtPath(path);
		if (importer != null)
		{
			importer.assetBundleName = string.Empty;
			importer.SaveAndReimport();
		}
	}

	public static void ClearAssetBundle<T>(List<T> objList) where T : UnityEngine.Object
	{
		foreach (T iterObject in objList)
		{
			ClearAssetBundle(iterObject);
		}
	}

	public static List<T> CollectAll<T>(string path) where T : UnityEngine.Object
	{
		List<T> l = new List<T>();
		string[] files = Directory.GetFiles(path);

		foreach (string file in files)
		{
			if (file.Contains(".meta")) continue;
			T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
			if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
			l.Add(asset);
		}
		return l;
	}

	public static List<T> CollectAll<T>(string path, string filter) where T : UnityEngine.Object
	{
		List<T> l = new List<T>();
		string[] files = Directory.GetFiles(path, filter);

		foreach (string file in files)
		{
			if (file.Contains(".meta")) continue;
			T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
			if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
			l.Add(asset);
		}
		return l;
	}

	public static Byte[] ReadFile(string file)
	{
		if (File.Exists(file) == false)
		{
			Debug.Log("Not find file: " + file);
			return null;
		}
		FileStream br = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
		if (br == null)
		{
			return null;
		}
		Byte[] data = new Byte[br.Length];
		br.Read(data, 0, data.Length);
		br.Close();
		return data;
	}

	public static void SaveFile(string file, Byte[] data)
	{
		FileStream bw = File.Create(file);
		if (bw != null)
		{
			bw.Write(data, 0, data.Length);
			bw.Close();
		}
	}
	public static bool Equals(Byte[] left, Byte[] right)
	{
		if (left == null || right == null)
		{
			return false;
		}
		if (left.Length != right.Length)
		{
			return false;
		}
		for (int iter = 0; iter < left.Length; ++iter)
		{
			if (left[iter] != right[iter])
			{
				return false;
			}
		}
		return true;
	}
	public static string MD5_Content(Byte[] content)
	{
		MD5 md5 = MD5.Create();
		Byte[] data = md5.ComputeHash(content);
		StringBuilder sBuilder = new StringBuilder();
		for (int i = 0; i < data.Length; i++)
		{
			sBuilder.Append(data[i].ToString("X2"));
		}
		return sBuilder.ToString();
	}
}
