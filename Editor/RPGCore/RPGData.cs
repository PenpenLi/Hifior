using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace RPGEditor
{
    public class RPGData : Editor
    {
        public static List<string> CharacterNameList = new List<string>();
        public static List<string> CareerNameList = new List<string>();
        public static List<string> WeaponNameList = new List<string>();
        public static List<string> PropNameList = new List<string>();

        //[UnityEditor.Callbacks.DidReloadScripts]
        static RPGData()
        {
            RefreshDataBase();
        }
        [MenuItem("RPGEditor/RefreshDataBase", false, 100)]
        public static void RefreshDataBase()
        {
            CharacterNameList.Clear();
            CareerNameList.Clear();
            WeaponNameList.Clear();
            PropNameList.Clear();

            RefreshData(PlayerDefEditor.DIRECTORY_PATH, CharacterNameList, (string s) => { return AssetDatabase.LoadAssetAtPath<CharacterDef>(s).CommonProperty.Name; });
            RefreshData(CareerDefEditor.DIRECTORY_PATH, CareerNameList, (string s) => { return AssetDatabase.LoadAssetAtPath<CareerDef>(s).CommonProperty.Name; });
            RefreshData(WeaponDefEditor.DIRECTORY_PATH, WeaponNameList, (string s) => { return AssetDatabase.LoadAssetAtPath<WeaponDef>(s).CommonProperty.Name; });
            RefreshData(PropsDefEditor.DIRECTORY_PATH, PropNameList, (string s) => { return AssetDatabase.LoadAssetAtPath<PropsDef>(s).CommonProperty.Name; });
        }
        delegate string DelegateGetName(string name);
        static void RefreshData(string path, List<string> nameList, DelegateGetName get)
        {
            if (Directory.Exists(path))
            {
                string[] files = ScriptableObjectUtility.GetFiles(path, "asset");
                for (int i = 0; i < files.Length; i++)
                {
                    string name = get(files[i]);
                    nameList.Add(name);
                }
            }
            else
            {
                Debug.LogError("指定位置的目录不存在：" + path);
            }
        }
        /// <summary>
        /// 载入路径下所有继承ScriptableObject的T类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TList"></param>
        /// <param name="path"></param>
        /// <param name="postfix"></param>
        public static void LoadDefAssetAtPath<T>(ref List<T> TList, string path, string postfix) where T : ScriptableObject
        {
            if (TList == null)
            {
                TList = new List<T>();
            }
            TList.Clear();
            string[] defs = (ScriptableObjectUtility.GetFiles(path, postfix));
            foreach (string defFileName in defs)
            {
                TList.Add(AssetDatabase.LoadAssetAtPath<T>(defFileName));
            }
        }
    }
}
