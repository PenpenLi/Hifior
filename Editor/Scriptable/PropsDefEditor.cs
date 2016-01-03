using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(PropsDef))]
    public class PropsDefEditor : Editor
    {
        private static int dedicatedJobCount;
        private static int dedicatedCharacterCount;
        private static bool foldout_AdditionalAttribute = true;
        private static bool foldout_AdditionalAttiburteGrow = true;
        public const string DIRECTORY_PATH = "Assets/RPG Data/Items/Props";
        [MenuItem("RPGEditor/Create Items/Props", false, 2)]
        public static PropsDef CreateProps()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            PropsDef props = ScriptableObjectUtility.CreateAsset<PropsDef>(
                count.ToString(),
                DIRECTORY_PATH,
                true
            );
            props.CommonProperty.ID = count;
            props.CommonProperty.Name = "伤药";
            return props;
        }
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "武器的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("武器名", "武器的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("武器描述", "武器的详细描述介绍");
        private readonly GUIContent guiContent_ImportantWeapon = new GUIContent("神器", "是否是很强大的特殊武器");
        private readonly GUIContent guiContent_NoExchange = new GUIContent("不可交换", "该武器不可以和其他人进行交换操作");
        PropsDef props;
        public override void OnInspectorGUI()
        {
            //在最开始写Label会显示在最上面
            Rect blockLabelRect = new Rect(45, 5, 120, 16);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("道具"), RPGEditorGUI.CenterLabelStyle);
            //EditorGUI.InspectorTitlebar(new Rect(5, 5, 128, 128),true , new Object[] { Resources.Load("CharIcon/0_0") },true );
            if (props.Icon != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), props.Icon.texture);
            props.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, props.CommonProperty.ID);
            props.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, props.CommonProperty.Name);
            props.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, props.CommonProperty.Description);

            props.Icon = (Sprite)EditorGUILayout.ObjectField("图标", props.Icon, typeof(Sprite), false);
            props.SinglePrice = EditorGUILayout.IntField("单价", props.SinglePrice);
            props.UseNumber = EditorGUILayout.IntField("使用次数", props.UseNumber);

            props.PropsEffect = (EnumPropsEffectType)EditorGUILayout.EnumPopup("道具效果", props.PropsEffect);
            props.Power = EditorGUILayout.IntField("值", props.Power);

            string[] display = DataDef.CareerNameList.ToArray();
            int[] value = EnumTables.GetSequentialArray(DataDef.CareerNameList.Count);
            RPGEditorGUI.DynamicArrayView(ref dedicatedCharacterCount, ref props.DedicatedCharacter, "专用人物", "人物", display, value);
            RPGEditorGUI.DynamicArrayView(ref dedicatedJobCount, ref props.DedicatedJob, "专用职业", "职业", display, value);

            props.ImportantProps = EditorGUILayout.Toggle(guiContent_ImportantWeapon, props.ImportantProps);
            props.NoExchange = EditorGUILayout.Toggle(guiContent_NoExchange, props.NoExchange);

            foldout_AdditionalAttribute = EditorGUILayout.Foldout(foldout_AdditionalAttribute, "人物属性修正");
            if (foldout_AdditionalAttribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                props.AdditionalAttribute.HP = EditorGUILayout.IntSlider("HP", props.AdditionalAttribute.HP, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", props.AdditionalAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", props.AdditionalAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.Skill = EditorGUILayout.IntSlider("技术", props.AdditionalAttribute.Skill, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.Speed = EditorGUILayout.IntSlider("速度", props.AdditionalAttribute.Speed, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.Lucky = EditorGUILayout.IntSlider("幸运", props.AdditionalAttribute.Lucky, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", props.AdditionalAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", props.AdditionalAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);
                props.AdditionalAttribute.Movement = EditorGUILayout.IntSlider("移动", props.AdditionalAttribute.Movement, 0, RPGEditorGlobal.MAX_ADDATTIBUTE);

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

                props.AdditionalAttributeGrow.HP = EditorGUILayout.IntSlider("HP成长率", props.AdditionalAttributeGrow.HP, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.PhysicalPower = EditorGUILayout.IntSlider("物理攻击成长率", props.AdditionalAttributeGrow.PhysicalPower, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.MagicalPower = EditorGUILayout.IntSlider("魔法攻击成长率", props.AdditionalAttributeGrow.MagicalPower, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.Skill = EditorGUILayout.IntSlider("技术成长率", props.AdditionalAttributeGrow.Skill, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.Speed = EditorGUILayout.IntSlider("速度成长率", props.AdditionalAttributeGrow.Speed, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.Lucky = EditorGUILayout.IntSlider("幸运成长率", props.AdditionalAttributeGrow.Lucky, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.PhysicalDefense = EditorGUILayout.IntSlider("物理防御成长率", props.AdditionalAttributeGrow.PhysicalDefense, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.MagicalDefense = EditorGUILayout.IntSlider("魔法防御成长率", props.AdditionalAttributeGrow.MagicalDefense, 0, RPGEditorGlobal.MAX_ADDGROW);
                props.AdditionalAttributeGrow.Movement = EditorGUILayout.IntSlider("移动成长率", props.AdditionalAttributeGrow.Movement, 0, RPGEditorGlobal.MAX_ADDGROW);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            if (GUI.changed)
                EditorUtility.SetDirty(target);

        }
        public void OnEnable()
        {
            props = target as PropsDef;
            dedicatedCharacterCount = props.DedicatedCharacter.Count;
            dedicatedJobCount = props.DedicatedJob.Count;
        }
    }
}