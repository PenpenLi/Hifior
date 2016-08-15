using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(CareerDef))]
    public class CareerDefEditor : Editor
    {
        private static bool foldout_UseableWeapon = true;
        private static bool foldout_MaxAttribute = true;
        private static string[] weaponLevelNames;
        private static int weaponLevelCount = 0;
        private static bool[] WeaponEnableTable;
        public const string DIRECTORY_PATH = DataBaseConst.DataBase_Career_Folder;
        [MenuItem("RPGEditor/Create Career", false, 1)]
        public static CareerDef CreateProps()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            CareerDef career = ScriptableObjectUtility.CreateAsset<CareerDef>(count.ToString(), DIRECTORY_PATH, true);
            career.CommonProperty.ID = count;
            career.CommonProperty.Name = "剑士";
            return career;
        }
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "职业的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("职业名称", "职业的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("职业描述", "职业的详细描述介绍");

        CareerDef career;
        public override void OnInspectorGUI()
        {
            //在最开始写Label会显示在最上面
            Rect blockLabelRect = new Rect(45, 5, 120, 55);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("职业"), RPGEditorGUI.Head1Style);
            //EditorGUI.InspectorTitlebar(new Rect(5, 5, 128, 128),true , new Object[] { Resources.Load("CharIcon/0_0") },true );
            if (career.Icon != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), career.Icon.texture);
            career.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, career.CommonProperty.ID);
            career.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, career.CommonProperty.Name);
            career.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, career.CommonProperty.Description);

            career.Icon = (Sprite)EditorGUILayout.ObjectField("图标", career.Icon, typeof(Sprite), false);
            career.Level = (EnumCareerLevel)EditorGUILayout.EnumPopup("职业等级", career.Level);
            career.Series = (EnumCareerSeries)EditorGUILayout.EnumPopup("职业系", career.Series);

            career.Skill = EditorGUILayout.MaskField("职业特有技能", career.Skill, new string[] { "再次移动", "必杀+30", "魔物特效" });
            career.ModelSize = (EnumCareerModelSize)EditorGUILayout.EnumPopup("模型大小", career.ModelSize);

            foldout_UseableWeapon = EditorGUILayout.Foldout(foldout_UseableWeapon, "职业可用武器");
            if (foldout_UseableWeapon)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                for (int i = 0; i < weaponLevelCount; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    WeaponEnableTable[i] = EditorGUILayout.Toggle(weaponLevelNames[i], WeaponEnableTable[i]);
                    if (WeaponEnableTable[i])
                    {
                        if (career.UseWeaponTypeLevel[i] < 0)
                            career.UseWeaponTypeLevel[i] = 0;
                        //editorWeaponLevel[i] = (EnumWeaponLevel)EditorGUILayout.EnumPopup(editorWeaponLevel[i]);
                        //career.UseWeaponTypeLevel[i] = (int)editorWeaponLevel[i];
                        career.UseWeaponTypeLevel[i] = (int)(EnumWeaponLevel)EditorGUILayout.EnumPopup((EnumWeaponLevel)career.UseWeaponTypeLevel[i]);
                    }
                    else
                    {
                        career.UseWeaponTypeLevel[i] = -1;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            foldout_MaxAttribute = EditorGUILayout.Foldout(foldout_MaxAttribute, "职业最大属性");
            if (foldout_MaxAttribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                career.MaxAttribute.HP = EditorGUILayout.IntSlider("HP", career.MaxAttribute.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                career.MaxAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", career.MaxAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", career.MaxAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.Skill = EditorGUILayout.IntSlider("技术", career.MaxAttribute.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.Speed = EditorGUILayout.IntSlider("速度", career.MaxAttribute.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.Luck = EditorGUILayout.IntSlider("幸运", career.MaxAttribute.Luck, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", career.MaxAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", career.MaxAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                career.MaxAttribute.Movement = EditorGUILayout.IntSlider("移动", career.MaxAttribute.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        public void OnEnable()
        {
            career = target as CareerDef;
            weaponLevelNames = System.Enum.GetNames(typeof(EnumWeaponType));
            weaponLevelCount = weaponLevelNames.Length;
            if (career.UseWeaponTypeLevel == null)
            {
                career.UseWeaponTypeLevel = new int[weaponLevelCount];
                career.MaxAttribute.SetMaxium();
            }
            if (WeaponEnableTable == null)
            {
                WeaponEnableTable = new bool[weaponLevelCount];
                for (int i = 0; i < weaponLevelCount; i++)
                {
                    WeaponEnableTable[i] = career.UseWeaponTypeLevel[i] != -1;
                }
            }
        }
    }

    public class CareerEditorProp : EditorProp<CareerDef>
    {
        private static bool foldout_UseableWeapon = true;
        private static bool foldout_MaxAttribute = true;
        private static string[] weaponLevelNames;
        private static int weaponLevelCount = 0;
        private static bool[] WeaponEnableTable;
        public const string DIRECTORY_PATH = DataBaseConst.DataBase_Career_Folder;

        private readonly GUIContent guiContent_ID = new GUIContent("ID", "职业的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("职业名称", "职业的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("职业描述", "职业的详细描述介绍");
        public override string AssetFolder
        {
            get
            {
                return DataBaseConst.ScriptableObjectBasePath + "Career";
            }
        }

        public override string ListName(int index)
        {
            string Name = scriptableObjects[index].CommonProperty.Name;
            if (string.IsNullOrEmpty(Name.Trim()))
                return base.NO_NAME;
            return Name;
        }

        public override void OnGUI(CareerDef Data)
        {
            Data.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, Data.CommonProperty.ID);
            Data.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, Data.CommonProperty.Name);
            Data.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, Data.CommonProperty.Description);

            Data.Icon = (Sprite)EditorGUILayout.ObjectField("图标", Data.Icon, typeof(Sprite), false);
            Data.Level = (EnumCareerLevel)EditorGUILayout.EnumPopup("职业等级", Data.Level);
            Data.Series = (EnumCareerSeries)EditorGUILayout.EnumPopup("职业系", Data.Series);

            Data.Skill = EditorGUILayout.MaskField("职业特有技能", Data.Skill, new string[] { "再次移动", "必杀+30", "魔物特效" });
            Data.ModelSize = (EnumCareerModelSize)EditorGUILayout.EnumPopup("模型大小", Data.ModelSize);

            foldout_UseableWeapon = EditorGUILayout.Foldout(foldout_UseableWeapon, "职业可用武器");
            if (foldout_UseableWeapon)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                for (int i = 0; i < weaponLevelCount; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    WeaponEnableTable[i] = EditorGUILayout.Toggle(weaponLevelNames[i], WeaponEnableTable[i]);
                    if (WeaponEnableTable[i])
                    {
                        if (Data.UseWeaponTypeLevel[i] < 0)
                            Data.UseWeaponTypeLevel[i] = 0;
                        //editorWeaponLevel[i] = (EnumWeaponLevel)EditorGUILayout.EnumPopup(editorWeaponLevel[i]);
                        //career.UseWeaponTypeLevel[i] = (int)editorWeaponLevel[i];
                        Data.UseWeaponTypeLevel[i] = (int)(EnumWeaponLevel)EditorGUILayout.EnumPopup((EnumWeaponLevel)Data.UseWeaponTypeLevel[i]);
                    }
                    else
                    {
                        Data.UseWeaponTypeLevel[i] = -1;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            foldout_MaxAttribute = EditorGUILayout.Foldout(foldout_MaxAttribute, "职业最大属性");
            if (foldout_MaxAttribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                Data.MaxAttribute.HP = EditorGUILayout.IntSlider("HP", Data.MaxAttribute.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                Data.MaxAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", Data.MaxAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", Data.MaxAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.Skill = EditorGUILayout.IntSlider("技术", Data.MaxAttribute.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.Speed = EditorGUILayout.IntSlider("速度", Data.MaxAttribute.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.Luck = EditorGUILayout.IntSlider("幸运", Data.MaxAttribute.Luck, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", Data.MaxAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", Data.MaxAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.MaxAttribute.Movement = EditorGUILayout.IntSlider("移动", Data.MaxAttribute.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}