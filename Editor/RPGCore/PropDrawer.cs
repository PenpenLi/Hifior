using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPGEditor
{
    public class PropDrawer<T> : EditorWindowDrawer where T : ScriptableObject, new()
    {
        private Vector2[] scroll=new Vector2[] { Vector2.zero,Vector2.zero};
        public bool IsShowing;
        private static int selected;

        public EditorProp<T> Drawer;
        public PropDrawer(EditorProp<T> fd)
        {
            Drawer = fd;
        }
        public void OnLoad()
        {
            Drawer.OnEnable();
        }

        public virtual GUIStyle ListStyle
        {
            get
            {
                return RPGEditorGUI.RichLabelStyle;
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {

                scroll[0] = EditorGUILayout.BeginScrollView(scroll[0],false,true);
                {
                    for (int i = 0; i < Drawer.scriptableObjects.Count; i++)
                    {
                        if (GUILayout.Button(Drawer.ListName(i), ListStyle, GUILayout.Width(200)))
                        {
                            selected = i;
                        }
                    }
                }
                EditorGUILayout.EndScrollView();

                scroll[1] = EditorGUILayout.BeginScrollView(scroll[1],false,false);
                Drawer.OnGUI(Drawer.scriptableObjects[selected]);
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
