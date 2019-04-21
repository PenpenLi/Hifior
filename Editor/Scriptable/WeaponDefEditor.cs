using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(WeaponDef))]
    public class WeaponDefEditor : Editor
    {
        private static int dedicatedJobCount;
        private static int dedicatedCharacterCount;
        private static int careerEffectCount;
        private static bool foldout_Range = true;
        private static bool foldout_AdditionalAttribute = true;
        private static bool foldout_AdditionalAttiburteGrow = true;
        public const string DIRECTORY_PATH = DataBaseConst.DataBase_Weapon_Folder;
        [MenuItem("RPGEditor/Create Items/Weapon", false)]
        public static WeaponDef CreateWeapon()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            WeaponDef wea = ScriptableObjectUtility.CreateAsset<WeaponDef>(
                count.ToString(),
                DIRECTORY_PATH,
                true
            );
            wea.CommonProperty.ID = count;
            wea.CommonProperty.Name = "青铜剑";
            return wea;
        }
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "武器的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("武器名", "武器的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("武器描述", "武器的详细描述介绍");
        private readonly GUIContent guiContent_SuperEffect = new GUIContent("超级特效", "具备哪些特殊的效果");
        private readonly GUIContent guiContent_ImportantWeapon = new GUIContent("神器", "是否是很强大的特殊武器");
        private readonly GUIContent guiContent_NoExchange = new GUIContent("不可交换", "该武器不可以和其他人进行交换操作");
        WeaponDef wea;
        public override void OnInspectorGUI()
        {
            Rect blockLabelRect = new Rect(45, 5, 120, 16);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("武器"), RPGEditorGUI.CenterLabelStyle);
            //EditorGUI.InspectorTitlebar(new Rect(5, 5, 128, 128),true , new Object[] { Resources.Load("CharIcon/0_0") },true );
            if (wea.Icon != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), wea.Icon.texture);

            wea.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, wea.CommonProperty.ID);
            wea.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, wea.CommonProperty.Name);
            wea.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, wea.CommonProperty.Description);

            wea.Icon = (Sprite)EditorGUILayout.ObjectField("图标", wea.Icon, typeof(Sprite), false);
            wea.WeaponType = (EnumWeaponType)EditorGUILayout.EnumPopup("武器类型", wea.WeaponType);
            wea.WeaponLevel = (EnumWeaponLevel)EditorGUILayout.EnumPopup("武器等级", wea.WeaponLevel);
            wea.SinglePrice = EditorGUILayout.IntField("单价", wea.SinglePrice);
            wea.UseNumber = EditorGUILayout.IntField("使用次数", wea.UseNumber);
            //根据枚举类型决定该以何种形式呈现
            foldout_Range = EditorGUILayout.Foldout(foldout_Range, "武器攻击范围");
            if (foldout_Range)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                wea.RangeType.SelectType = (EnumSelectEffectRangeType)EditorGUILayout.EnumPopup("选择范围类型", wea.RangeType.SelectType);
                wea.RangeType.EffectType = (EnumSelectEffectRangeType)EditorGUILayout.EnumPopup("生效范围类型", wea.RangeType.EffectType);
                wea.RangeType.SelectRange = EditorGUILayout.Vector2IntField("选择距离", wea.RangeType.SelectRange);
                wea.RangeType.EffectRange = EditorGUILayout.Vector2IntField("生效距离", wea.RangeType.EffectRange);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            //////////
            wea.Weight = EditorGUILayout.IntField("重量", wea.Weight);
            wea.Power = EditorGUILayout.IntField("威力", wea.Power);
            wea.Hit = EditorGUILayout.IntField("命中率", wea.Hit);
            wea.Crit = EditorGUILayout.IntField("必杀率", wea.Crit);

            RPGEditorGUI.DynamicArrayView(ref dedicatedCharacterCount, ref wea.DedicatedCharacter, "专用人物", "人物");
            string[] display = RPGData.CareerNameList.ToArray();
            int[] value = EnumTables.GetSequentialArray(RPGData.CareerNameList.Count);
            RPGEditorGUI.DynamicArrayView(ref dedicatedJobCount, ref wea.DedicatedJob, "专用职业", "职业", display, value);
            RPGEditorGUI.DynamicArrayView(ref careerEffectCount, ref wea.CareerEffect, "克制职业", "职业", display, value);

            //Log.Write(EnumTables.MaskFieldIdentify(wea.CareerEffect, 0), EnumTables.MaskFieldIdentify(wea.CareerEffect, 1), EnumTables.MaskFieldIdentify(wea.CareerEffect, 2), EnumTables.MaskFieldIdentify(wea.CareerEffect, 3));
            //Log.Write(EnumTables.MaskFieldSetTrue(2, 1));
            //Log.Write(EnumTables.MaskFieldSetFalse(2, 0));

            wea.SuperEffect = EditorGUILayout.MaskField(guiContent_SuperEffect, wea.SuperEffect, new string[] { "必杀", "不可反击", "狂乱", "吸血" });
            wea.AttackEffect = (EnumWeaponAttackEffectType)EditorGUILayout.EnumPopup("特殊攻击效果", wea.AttackEffect);
            wea.ImportantWeapon = EditorGUILayout.Toggle(guiContent_ImportantWeapon, wea.ImportantWeapon);
            wea.NoExchange = EditorGUILayout.Toggle(guiContent_NoExchange, wea.NoExchange);

            foldout_AdditionalAttribute = EditorGUILayout.Foldout(foldout_AdditionalAttribute, "人物属性修正");
            if (foldout_AdditionalAttribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                wea.AdditionalAttribute.HP = EditorGUILayout.IntSlider("HP", wea.AdditionalAttribute.HP, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", wea.AdditionalAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", wea.AdditionalAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.Skill = EditorGUILayout.IntSlider("技术", wea.AdditionalAttribute.Skill, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.Speed = EditorGUILayout.IntSlider("速度", wea.AdditionalAttribute.Speed, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.Luck = EditorGUILayout.IntSlider("幸运", wea.AdditionalAttribute.Luck, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", wea.AdditionalAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", wea.AdditionalAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                wea.AdditionalAttribute.Movement = EditorGUILayout.IntSlider("移动", wea.AdditionalAttribute.Movement, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            foldout_AdditionalAttiburteGrow = EditorGUILayout.Foldout(foldout_AdditionalAttiburteGrow, "成长率修正");
            //人物成长率修正
            if (foldout_AdditionalAttiburteGrow)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                wea.AdditionalAttributeGrow.HP = EditorGUILayout.IntSlider("HP成长率", wea.AdditionalAttributeGrow.HP, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.PhysicalPower = EditorGUILayout.IntSlider("物理攻击成长率", wea.AdditionalAttributeGrow.PhysicalPower, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.MagicalPower = EditorGUILayout.IntSlider("魔法攻击成长率", wea.AdditionalAttributeGrow.MagicalPower, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.Skill = EditorGUILayout.IntSlider("技术成长率", wea.AdditionalAttributeGrow.Skill, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.Speed = EditorGUILayout.IntSlider("速度成长率", wea.AdditionalAttributeGrow.Speed, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.Luck = EditorGUILayout.IntSlider("幸运成长率", wea.AdditionalAttributeGrow.Luck, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.PhysicalDefense = EditorGUILayout.IntSlider("物理防御成长率", wea.AdditionalAttributeGrow.PhysicalDefense, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.MagicalDefense = EditorGUILayout.IntSlider("魔法防御成长率", wea.AdditionalAttributeGrow.MagicalDefense, 0, RPGEditorGlobal.MAX_ADDGROW);
                wea.AdditionalAttributeGrow.Movement = EditorGUILayout.IntSlider("移动成长率", wea.AdditionalAttributeGrow.Movement, 0, RPGEditorGlobal.MAX_ADDGROW);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
        public void OnEnable()
        {
            wea = target as WeaponDef;
            if (wea.DedicatedCharacter == null)
                wea.DedicatedCharacter = new System.Collections.Generic.List<int>();
            dedicatedCharacterCount = wea.DedicatedCharacter.Count;
            dedicatedJobCount = wea.DedicatedJob.Count;
            careerEffectCount = wea.CareerEffect.Count;
        }
    }
}