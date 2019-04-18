using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
namespace RPGEditor
{
    public class EnemyUnitSettingWindow : EditorWindow
    {
        /// <summary>
        /// 修改还是添加
        /// </summary>
        private static bool m_bMedify = false;
        private Vector2[] scroll = { Vector2.zero, Vector2.zero };
        /// <summary>
        /// 保存敌方人物的属性列表
        /// </summary>
        private static List<EnemyDef> EnemyDefList;
        /// <summary>
        /// 当前选定的人物属性
        /// </summary>
        private static EnemyDef ActiveEnemyDef;
        /// <summary>
        /// 选定的Index
        /// </summary>
        public static int ActiveIndex
        {
            private set;
            get;
        }
        public static bool IsShowing
        {
            get;
            private set;
        }
        private static int previousX;
        private static int previousY;
        /// <summary>
        /// 单个敌方单位
        /// </summary>
        private static EnemyUnitSetting.EnemyUnit unit;
        /// <summary>
        /// 实际的地方设定文件
        /// </summary>
        private static EnemyUnitSetting UnitSetting;
        /// <summary>
        /// 打开添加敌人的窗口
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void OpenAddEnemyWindow(int x, int y, EnemyUnitSetting setting)
        {
            OpenWindow();
            m_bMedify = false;
            UnitSetting = setting;
            unit = new EnemyUnitSetting.EnemyUnit(x, y);
        }
        public static void OpenSetEnemyWindow(int x, int y, EnemyUnitSetting setting, EnemyDef enemy, int index)
        {
            OpenWindow();
            m_bMedify = true;
            previousX = x;
            previousY = y;

            UnitSetting = setting;
            unit = new EnemyUnitSetting.EnemyUnit(x, y);
            unit.Enemy = enemy;
            if (enemy == null)
                unit.Enemy = EnemyDefList[0];
            ActiveIndex = index;
        }
        static void OpenWindow()
        {
            IsShowing = true;
            EnemyUnitSettingWindow mapEditor = EditorWindow.GetWindow<EnemyUnitSettingWindow>();
            mapEditor.ShowPopup();
            if (EnemyDefList.Count == 0)
            {
                Debug.LogError("没有EnemyDef文件");
            }
            else
                unit.Enemy = EnemyDefList[0];
        }
        /// <summary>
        /// 窗口打开需要初始化EnemyDef数据库
        /// </summary>
        void Awake()
        {
            if (EnemyDefList == null)
                RPGData.LoadDefAssetAtPath<EnemyDef>(ref EnemyDefList, EnemyDefEditor.DIRECTORY_PATH, "asset");
        }
        void OnFocus()
        {
            if (EnemyDefList.Count > 0)
            {
                ActiveIndex = 0;
                ActiveEnemyDef = EnemyDefList[0];
            }
        }
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("刷新数据", GUILayout.Width(120)))
            {
                RPGData.LoadDefAssetAtPath<EnemyDef>(ref EnemyDefList, EnemyDefEditor.DIRECTORY_PATH, "asset");
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("确定", GUILayout.Width(120)))
            {
                if (!IsValidUnit())
                {
                    EditorUtility.DisplayDialog("无效坐标", "插入的坐标已经存在或格式不正确", "OK");
                    return;
                }
                if (!m_bMedify)
                    UnitSetting.Units.Add(unit);
                else
                {
                    EnemyUnitSetting.EnemyUnit previousUnit = UnitSetting.GetUnit(new Vector2Int(previousX, previousY));
                    int i = UnitSetting.Units.IndexOf(previousUnit);
                    if (i >= 0)
                    {
                        UnitSetting.Units.RemoveAt(i);
                        UnitSetting.Units.Insert(i, new EnemyUnitSetting.EnemyUnit(unit.Coord, unit.Enemy));
                    }
                }
                Close();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("取消", GUILayout.Width(120)))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            ShowEnemyDefList();
            RPGEditorGUI.DrawVerticalLine(2, Color.black);
            ShowEnemyDefContent();
            EditorGUILayout.EndHorizontal();
            Undo.RecordObject(UnitSetting, "EnemyUnitSetting");
        }
        void ShowEnemyDefList()
        {
            scroll[0] = EditorGUILayout.BeginScrollView(scroll[0], GUILayout.MinWidth(120));
            {
                if (EnemyDefList == null)
                    return;
                for (int i = 0; i < EnemyDefList.Count; i++)
                {
                    string enemyName = EnemyDefList[i].CommonProperty.Name;
                    if (enemyName == null || enemyName.Equals(""))
                        enemyName = "无名敌人";
                    if (GUILayout.Button(enemyName, i == ActiveIndex ? RPGEditorGUI.ListItemSelectedStyle : RPGEditorGUI.ListItemBackLightStyle, GUILayout.MaxWidth(150)))
                    {
                        ActiveIndex = i;
                        ActiveEnemyDef = EnemyDefList[i];
                    }
                }
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }
        void ShowEnemyDefContent()
        {
            if (ActiveEnemyDef == null)
                return;

            unit.Enemy = ActiveEnemyDef;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
            {
                EditorGUILayout.LabelField("坐标", GUILayout.Width(50));
                unit.Coord.x = EditorGUILayout.IntField("x", unit.Coord.x, GUILayout.MaxWidth(200));
                unit.Coord.y = EditorGUILayout.IntField("y", unit.Coord.y, GUILayout.MaxWidth(200));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            unit.Enemy.CommonProperty.ID = EditorGUILayout.IntField("人物ID", unit.Enemy.CommonProperty.ID);
            unit.Enemy.CommonProperty.Name = EditorGUILayout.TextField("人物名称", unit.Enemy.CommonProperty.Name);
            unit.Enemy.CommonProperty.Description = EditorGUILayout.TextField("人物描述", unit.Enemy.CommonProperty.Description);

            unit.Enemy.PlayerDef.Portrait = (Sprite)EditorGUILayout.ObjectField("图标", unit.Enemy.PlayerDef.Portrait, typeof(Sprite), false);
            unit.Enemy.PlayerDef.BattleModel = EditorGUILayout.ObjectField("人物模型", unit.Enemy.PlayerDef.BattleModel, typeof(GameObject), true) as GameObject;
            unit.Enemy.PlayerDef.CharacterImportance = (EnumCharacterImportance)EditorGUILayout.EnumPopup("重要性", unit.Enemy.PlayerDef.CharacterImportance);
            unit.Enemy.PlayerDef.Career = EditorGUILayout.IntPopup("职业", unit.Enemy.PlayerDef.Career, RPGData.CareerNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.CareerNameList.Count));
            unit.Enemy.PlayerDef.DefaultLevel = EditorGUILayout.IntSlider("初始等级", unit.Enemy.PlayerDef.DefaultLevel, 1, 40);

            unit.Enemy.PlayerDef.DefaultAttribute.HP = EditorGUILayout.IntSlider("HP", unit.Enemy.PlayerDef.DefaultAttribute.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
            unit.Enemy.PlayerDef.DefaultAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", unit.Enemy.PlayerDef.DefaultAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", unit.Enemy.PlayerDef.DefaultAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.Skill = EditorGUILayout.IntSlider("技术", unit.Enemy.PlayerDef.DefaultAttribute.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.Speed = EditorGUILayout.IntSlider("速度", unit.Enemy.PlayerDef.DefaultAttribute.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.Luck = EditorGUILayout.IntSlider("幸运", unit.Enemy.PlayerDef.DefaultAttribute.Luck, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", unit.Enemy.PlayerDef.DefaultAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", unit.Enemy.PlayerDef.DefaultAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
            unit.Enemy.PlayerDef.DefaultAttribute.Movement = EditorGUILayout.IntSlider("移动", unit.Enemy.PlayerDef.DefaultAttribute.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EndVertical();
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
        private bool IsValidUnit()
        {
            if (unit.Coord.x < 0 || unit.Coord.y < 0) return false;
            if (!m_bMedify)
            {
                if (UnitSetting.Contains(unit.Coord))
                    return false;
            }
            else
            {
                if (unit.Coord != new Vector2Int(previousX, previousY))
                {
                    if (UnitSetting.Contains(unit.Coord))
                        return false;
                }
            }
            return true;
        }
    }
}