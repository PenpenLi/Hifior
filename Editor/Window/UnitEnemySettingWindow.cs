using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace RPGEditor
{ 
    public class UnitEnemySettingWindow : EditorWindow
    {
        public static int x, y;
        public static bool IsShowing
        {
            get;
            private set;
        }
        public static void OpenWindow(int x,int y)
        {
            RefreshPoint(x, y);
            UnitEnemySettingWindow mapEditor = EditorWindow.GetWindow<UnitEnemySettingWindow>();
            mapEditor.ShowPopup();
            IsShowing = true;
        }
        void OnGUI()
        {
            EditorGUILayout.LabelField("坐标" + new Point2D(x, y));
        }
        void OnInspectorUpdate()
        {
            this.Repaint();
        }
        void OnDestroy()
        {
            AssetDatabase.SaveAssets();
            IsShowing = false;
        }
        void Awake()
        {

        }
        public static void RefreshPoint(int x, int y)
        {
            UnitEnemySettingWindow.x = x;
            UnitEnemySettingWindow.y = y;
        }
    }
}