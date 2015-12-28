using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace RPGEditor
{
    public class RefreshDataBaseEditor : Editor
    {
        public static List<string> CharacterNameList = new List<string>();
        public static List<string> CareerNameList = new List<string>();
        public static List<string> WeaponNameList = new List<string>();
        public static List<string> PropNameList = new List<string>();

        [UnityEditor.Callbacks.DidReloadScripts]
        static RefreshDataBaseEditor()
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

            /*if (Directory.Exists(CharacterDefEditor.DIRECTORY_PATH))
            {
                string[] files = ScriptableObjectUtility.GetFiles(CharacterDefEditor.DIRECTORY_PATH, "asset");
                for (int i = 0; i < files.Length; i++)
                {
                    CharacterDef def = AssetDatabase.LoadAssetAtPath<CharacterDef>(files[i]);
                    CharacterNameList.Add(def.CommonProperty.Name);
                }
            }
            else
            {
                Debug.LogError("指定位置的目录不存在：" + CharacterDefEditor.DIRECTORY_PATH);
            }

            if (Directory.Exists(CareerDefEditor.DIRECTORY_PATH))
            {
                string[] files = ScriptableObjectUtility.GetFiles(CareerDefEditor.DIRECTORY_PATH, "asset");
                for (int i = 0; i < files.Length; i++)
                {
                    CareerDef def = AssetDatabase.LoadAssetAtPath<CareerDef>(files[i]);
                    CareerNameList.Add(def.CommonProperty.Name);
                }
            }
            else
            {
                Debug.LogError("指定位置的目录不存在：" + CareerDefEditor.DIRECTORY_PATH);
            }

            if (Directory.Exists(WeaponDefEditor.DIRECTORY_PATH))
            {
                string[] files = ScriptableObjectUtility.GetFiles(WeaponDefEditor.DIRECTORY_PATH, "asset");
                for (int i = 0; i < files.Length; i++)
                {
                    WeaponDef def = AssetDatabase.LoadAssetAtPath<WeaponDef>(files[i]);
                    WeaponNameList.Add(def.CommonProperty.Name);
                }
            }
            else
            {
                Debug.LogError("指定位置的目录不存在：" + WeaponDefEditor.DIRECTORY_PATH);
            }

            if (Directory.Exists(PropsDefEditor.DIRECTORY_PATH))
            {
                string[] files = ScriptableObjectUtility.GetFiles(PropsDefEditor.DIRECTORY_PATH, "asset");
                for (int i = 0; i < files.Length; i++)
                {
                    PropsDef def = AssetDatabase.LoadAssetAtPath<PropsDef>(files[i]);
                    PropNameList.Add(def.CommonProperty.Name);
                }
            }
            else
            {
                Debug.LogError("指定位置的目录不存在：" + PropsDefEditor.DIRECTORY_PATH);
            }*/
            RefreshData(CharacterDefEditor.DIRECTORY_PATH, CharacterNameList, (string s) => { return AssetDatabase.LoadAssetAtPath<CharacterDef>(s).CommonProperty.Name; });
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
    }

}
