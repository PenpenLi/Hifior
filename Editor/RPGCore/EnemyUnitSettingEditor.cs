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
        /// <summary>
        /// 鼠标按下按钮时捕捉到的位置
        /// </summary>
        public static int x;
        public static int y;
        /// <summary>
        /// 待复制的镜像单位
        /// </summary>
        private static EnemyUnitSetting.EnemyUnit unitMirror;
        /// <summary>
        /// 被拖拽移动的单位
        /// </summary>
        private static EnemyUnitSetting.EnemyUnit unitDrag;
        public EnemyUnitSettingEditor()
        {
            setSceneMenu = new GenericMenu();
            setSceneMenu.AddItem(new GUIContent("配置敌人"), false, OnSetUnit);
            setSceneMenu.AddItem(new GUIContent("复制"), false, OnCopy);
            setSceneMenu.AddItem(new GUIContent("剪切"), false, OnCut);
            setSceneMenu.AddItem(new GUIContent("删除"), false, OnDelete);
        }
        public override void OnInspectorGUI()
        {
            m_editable = EditorGUILayout.Toggle("编辑", m_editable);

            foreach (EnemyUnitSetting.EnemyUnit u in unitSetting.Units)
            {
                if (u.Enemy == null)
                {
                    EditorGUILayout.HelpBox("你的单位配置中有单位的数据为空,错误的单位用红色显示", MessageType.Error);
                    break;
                }
            }
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
                //编辑状态下显示右键菜单
                if (Event.current.button < 2)
                {
                    Selection.activeObject = unitSetting.transform;

                    if (Physics.Raycast(ray, out rayHit))
                    {
                        Point2D p = SLGMap.Vector3ToPoint2D(rayHit.point);

                        x = Mathf.Clamp(p.x, 0, int.MaxValue);
                        y = Mathf.Clamp(p.y, 0, int.MaxValue);
                        //按下右键弹出菜单
                        if (Event.current.button == 1)
                        {
                            if (unitSetting.Contains(x, y))
                                setSceneMenu.ShowAsContext();
                            else {
                                addSceneMenu = new GenericMenu();
                                addSceneMenu.AddItem(new GUIContent("添加敌人"), false, OnAddUnit, 0);
                                if (unitSetting.IsEmpty(unitMirror))
                                    addSceneMenu.AddDisabledItem(new GUIContent("粘贴"));
                                else
                                    addSceneMenu.AddItem(new GUIContent("粘贴"), false, OnPaste);
                                addSceneMenu.ShowAsContext();
                            }
                        }
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
                UnitEnemySettingWindow.OpenAddEnemyWindow(x, y, unitSetting);
            }
        }
        void OnSetUnit()
        {
            EnemyDef def = unitSetting.GetDef(x, y);
            if (def == null)
                UnitEnemySettingWindow.OpenSetEnemyWindow(x, y, unitSetting, def, 0);
            else
                UnitEnemySettingWindow.OpenSetEnemyWindow(x, y, unitSetting, def, def.CommonProperty.ID);
        }
        void OnCopy()
        {
            unitMirror = unitSetting.GetUnit(x, y);
        }
        void OnPaste()
        {
            unitSetting.Units.Add(new EnemyUnitSetting.EnemyUnit(x, y, unitMirror.Enemy));
        }
        void OnCut()
        {
            unitMirror = unitSetting.GetUnit(x, y);
            unitSetting.Remove(new Point2D(x, y));
        }
        void OnDelete()
        {
            unitSetting.Remove(new Point2D(x, y));
        }
    }
}