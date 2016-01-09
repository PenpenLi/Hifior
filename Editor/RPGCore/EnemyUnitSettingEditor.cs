using UnityEditor;
using UnityEngine;

namespace RPGEditor
{
    [CustomEditor(typeof(EnemyUnitSetting))]
    public class EnemyUnitSettingEditor : Editor
    {
        private static bool m_editable;
        GenericMenu addSceneMenu;
        GenericMenu setSceneMenu;
        EnemyUnitSetting unitSetting;
        public static int x;
        public static int y;
        public EnemyUnitSettingEditor()
        {
            addSceneMenu = new GenericMenu();
            addSceneMenu.AddItem(new GUIContent("添加敌人"), false, OnAddUnit, 0);
            setSceneMenu = new GenericMenu();
            setSceneMenu.AddItem(new GUIContent("配置敌人"), false, OnSetUnit, 0);
        }
        public override void OnInspectorGUI()
        {
            m_editable = EditorGUILayout.Toggle("编辑", m_editable);
            DrawDefaultInspector();
        }
        void OnSceneGUI()
        {
            unitSetting = (EnemyUnitSetting)target; //注意必须要获取target ，否则该事件不执行

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(100, 100, 100, 100));

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit rayHit;
            if (m_editable)
            {
                //编辑状态下不可选定其他物体
                if (Event.current.button == 0)
                {
                    Selection.activeObject = unitSetting.transform;
                }
                //编辑状态下显示右键菜单
                if (Event.current.button == 1)
                {
                    if (Physics.Raycast(ray, out rayHit))
                    {
                        Point2D p = SLGMap.Vector3ToPoint2D(rayHit.point);

                        x = Mathf.Clamp(p.x, 0, int.MaxValue);
                        y = Mathf.Clamp(p.y, 0, int.MaxValue);
                        if (unitSetting.Contains(x, y))
                            setSceneMenu.ShowAsContext();
                        else
                            addSceneMenu.ShowAsContext();
                    }
                }
            }
            GUILayout.EndArea();
            Handles.EndGUI();
        }
        void OnAddUnit(System.Object userData)
        {
            if ((int)userData == 0)
            {
                UnitEnemySettingWindow.OpenAddEnemyWindow( x, y,unitSetting);
            }
        }
        void OnSetUnit(System.Object userData)
        {
            if ((int)userData == 0)
            {
                EnemyDef def = unitSetting.GetDef(x, y);
                UnitEnemySettingWindow.OpenSetEnemyWindow(x, y, unitSetting,def , def.CommonProperty.ID);
            }
        }
    }
}