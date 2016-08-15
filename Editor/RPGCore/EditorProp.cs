using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPGEditor
{
    /// <summary>
    /// 包含单个属性的窗口，此类加载某个文件夹下所有的ScriptableObject
    /// </summary>
    public abstract class EditorProp<T> where T : ScriptableObject
    {
        protected readonly string NO_NAME = "未命名";
        public List<T> scriptableObjects = new List<T>();
        public abstract string AssetFolder { get; }
        public abstract void OnGUI(T Data);
        public void OnEnable()
        {
            string[] s = ScriptableObjectUtility.GetFiles(AssetFolder, "asset");
            foreach (string ss in s)
            {
                T d = AssetDatabase.LoadAssetAtPath(ss, typeof(T)) as T;
                scriptableObjects.Add(d);
            }
            OnLoad();
        }
        public virtual void OnLoad() { }
        /// <summary>
        /// 重写此方法以获得列表的个性化显示
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract string ListName(int index);
    }

    public static class DrawDefaultEditor
    {
        public static void DrawInspector<T>(T O) where T : ScriptableObject, new()
        {
            FieldInfo[] infos = O.GetType().GetFields();

            for (int i = 0; i < infos.Length; i++)
            {
                GUIContent label;
                object[] guiattrs = infos[i].GetCustomAttributes(typeof(GUIContentAttribute), true);
                if (guiattrs.Length > 0)
                {
                    GUIContentAttribute Attribute_GUIContent = guiattrs[0] as GUIContentAttribute;
                    label = Attribute_GUIContent.GUIContent;
                }
                else
                {
                    label = new GUIContent(infos[i].Name);
                }

                if (infos[i].FieldType == typeof(int))
                {
                    object[] intsliderattrs = infos[i].GetCustomAttributes(typeof(IntSliderAttribute), true);
                    if (intsliderattrs.Length > 0)
                    {
                        IntSliderAttribute Attribute_IntSlider = intsliderattrs[0] as IntSliderAttribute;
                        int min = Attribute_IntSlider.Min;
                        int max = Attribute_IntSlider.Max;
                        infos[i].SetValue(O, EditorGUILayout.IntSlider(label, (int)infos[i].GetValue(O), min, max));
                    }
                    else
                    {
                        infos[i].SetValue(O, EditorGUILayout.IntField(label, (int)infos[i].GetValue(O)));
                    }
                }
                if (infos[i].FieldType == typeof(string))
                {
                    infos[i].SetValue(O, EditorGUILayout.TextField(label, (string)infos[i].GetValue(O)));
                }

                if (infos[i].FieldType == typeof(float))
                {
                    infos[i].SetValue(O, EditorGUILayout.FloatField(label, (float)infos[i].GetValue(O)));
                }
            }
        }
    }
}
