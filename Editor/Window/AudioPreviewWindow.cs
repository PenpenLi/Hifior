using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace RPGEditor
{
    public class AudioPreviewWindow : EditorWindow
    {
        private Vector2[] scroll = { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };

        private static readonly string[] MenuItems = { "BGM", "BGS", "ME", "SE" };
        private int selected = 0;
        private int del = -1;
        private int ren = -1;
        private int renaming = -1;
        [MenuItem("RPGEditor/AudioPreview", false, 200)]
        public static void OpenEditor()
        {
            AudioPreviewWindow audioEditor = EditorWindow.GetWindow<AudioPreviewWindow>();
            audioEditor.ShowPopup();
        }
        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                // menu (on left side of window)
                selected = RPGEditorGUI.MenuHorizontal(selected, MenuItems);

                scroll[0] = EditorGUILayout.BeginScrollView(scroll[0]);
                {
                    switch (selected)
                    {
                        case 0: ShowBGM(); break;
                        case 1: ShowBGS(); break;
                        case 2: ShowME(); break;
                        case 3: ShowSE(); break;
                    }
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndScrollView();//0
            }
            EditorGUILayout.EndHorizontal();
        }
        public void ShowBGM() { }
        public void ShowBGS() { }
        public void ShowME() { }
        public void ShowSE() { }
    }
}
