using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace RPGEditor
{
    public interface EditorWindowDrawer
    {
        void OnLoad();
        void OnGUI();
    }

    public class DataBaseWindow : EditorWindow
    {
        private static int selected = 0;
        public static List<EditorWindowDrawer> windows = new List<EditorWindowDrawer>();
        private static PropDrawer<CareerDef> CareerDefEditor;
        private static PropDrawer<PlayerDef> PlayerDefEditor;
        private static PropDrawer<EnemyDef> EnemyDefEditor;
        private static GameSettingDrawer GameSettingEditor;

        public static List<string> PlayerNameList
        {

            get
            {
                List<string> s = new List<string>();
                foreach (PlayerDef def in PlayerDefEditor.Drawer.scriptableObjects)
                {
                    s.Add(def.CommonProperty.Name);
                }
                return s;
            }
        }

        public static List<string> CareerNameList
        {

            get
            {
                List<string> s = new List<string>();
                foreach (CareerDef def in CareerDefEditor.Drawer.scriptableObjects)
                {
                    s.Add(def.CommonProperty.Name);
                }
                return s;
            }
        }

        private static readonly string[] MenuItems = { "职业", "角色","敌人" ,"设定"};

        static void InitDataBase()
        {
            windows.Clear();
            CareerDefEditor = new PropDrawer<CareerDef>(new CareerEditorProp());
            windows.Add(CareerDefEditor);

            PlayerDefEditor = new PropDrawer<PlayerDef>(new PlayerEditorProp());
            windows.Add(PlayerDefEditor);

            EnemyDefEditor = new PropDrawer<EnemyDef>(new EnemyEditorProp());
            windows.Add(EnemyDefEditor);

            GameSettingEditor = new GameSettingDrawer();
            windows.Add(GameSettingEditor);

            if (windows.Count > 0)
            {
                for (int i = 0; i < windows.Count; i++)
                {
                    windows[i].OnLoad();
                }
            }
        }

        [MenuItem("RPGEditor/Data Editor", false, 500)]
        public static void OpenEditor()
        {
            DataBaseWindow audioEditor = EditorWindow.GetWindow<DataBaseWindow>();
            audioEditor.ShowPopup();

                InitDataBase();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                selected = RPGEditorGUI.MenuHorizontal(selected, MenuItems);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (windows.Count == 0)
                return;
            windows[selected].OnGUI();
            GUILayout.FlexibleSpace();

        }
        public void OnFocus()
        {
        }
        public void OnDestroy()
        {
            AssetDatabase.SaveAssets();
        }
    }
}