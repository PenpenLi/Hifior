using UnityEditor;
using UnityEngine;

namespace RPGEditor
{
    [CustomEditor(typeof(EnemyUnitSetting))]
    public class EnemyUnitSettingEditor : Editor
    {
        GenericMenu addSceneMenu;
        public static int x;
        public static int y;
        public EnemyUnitSettingEditor()
        {
            addSceneMenu = new GenericMenu();
            addSceneMenu.AddItem(new GUIContent("添加敌人"), false, OnAddUnit, 0);
            addSceneMenu.AddItem(new GUIContent("剪切"), false, OnAddUnit, 1);
            addSceneMenu.AddItem(new GUIContent("复制"), false, OnAddUnit, 2);
        }
        void OnSceneGUI()
        {
            EnemyUnitSetting unit = (EnemyUnitSetting)target; //注意必须要获取target ，否则该事件不执行

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(100, 100, 100, 100));

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit rayHit;

            if (Event.current.button == 1)
            {
                if (Physics.Raycast(ray, out rayHit))
                {
                    Point2D p = SLGMap.Vector3ToPoint2D(rayHit.point);

                    x = Mathf.Clamp(p.x, 0, int.MaxValue);
                    y = Mathf.Clamp(p.y, 0, int.MaxValue);
                    GUILayout.Label("x:" + x + " y:" + y);
                    addSceneMenu.ShowAsContext();
                }
            }

            GUILayout.EndArea();
            Handles.EndGUI();
        }
        void OnAddUnit(System.Object userData)
        {
            if ((int)userData == 0)
            {
                Debug.Log("在 x:" + x + "y:" + y + "处添加敌人");
                UnitEnemySettingWindow.OpenWindow(x, y);
            }
        }
    }
}