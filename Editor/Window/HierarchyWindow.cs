using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
namespace RPGEditor
{
    [InitializeOnLoad]
    public class HierarchyWindow : EditorWindow
    {
        private static Type[] subTypeQuery;
        private static Dictionary<string, UnityEngine.Object> TextureMap = new Dictionary<string, UnityEngine.Object>();
        private const string path = "Assets/Editor/Res/Hierarchy/";

        static HierarchyWindow()
        {
            subTypeQuery = GetAllSubTypes(typeof(MonoBehaviour));
            EditorApplication.hierarchyWindowItemOnGUI += DrawThing;
            string[] objs = ScriptableObjectUtility.GetFiles(path, "jpg", "png", "tga");
            foreach (string obj in objs)
            {
                string s = ScriptableObjectUtility.GetFileNameFromPath(obj);
                Texture2D t = AssetDatabase.LoadAssetAtPath(obj, typeof(Texture2D)) as Texture2D;
                TextureMap.Add(s, t);
            }
        }

        private static float width;
        private static void SetDrawPosition(ref Rect area, int pos = 0)
        {
            int x = Mathf.Clamp(pos, 0, 3);
            area.x = width - (3 - x) * 16;
        }
        static void DrawThing(int id, Rect area)
        {
            var go = EditorUtility.InstanceIDToObject(id) as GameObject;
            if (go == null)
                return;
            if (go.transform.parent == null)
                width = area.width;

            area.width = 16;
            area.height = 16;

            DrawAttribute(go, ref area);
        }
        static void DrawAttribute(GameObject go, ref Rect area)
        {
            foreach (MonoBehaviour comp in go.GetComponents<MonoBehaviour>())
            {
                if (subTypeQuery.Contains(comp.GetType()))
                {
                    var attr = (HierarchyIconAttribute)Attribute.GetCustomAttribute(comp.GetType(), typeof(HierarchyIconAttribute));
                    if (TextureMap.ContainsKey(attr.TextureFileName))
                    {
                        SetDrawPosition(ref area, attr.Position);
                        GUI.DrawTexture(area, AssetDatabase.LoadAssetAtPath(path + attr.TextureFileName, typeof(Texture2D)) as Texture2D);
                    }
                }
            }
        }

        public static Type[] GetAllSubTypes(Type aBaseClass)
        {
            var result = new List<Type>();
            Assembly[] AS = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var A in AS)
            {
                Type[] types = A.GetTypes();
                foreach (var T in types)
                {
                    if (T.IsDefined(typeof(HierarchyIconAttribute), true))
                        result.Add(T);
                }
            }
            return result.ToArray();
        }
    }
}