using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WeaponDef))]
public class WeaponDefEditor : Editor
{
    private static bool foldout_Range = true;
    private static bool foldout_AdditionalAttribute = true;
    private static bool foldout_AdditionalAttiburteGrow = true;
    public const string weaponFilePath = "Assets/RPGEditor Data/Items/Weapon";
    [MenuItem("RPGEditor/Create Items/Weapon", false, 0)]
    public static WeaponDef CreateWeapon()
    {
        int count = ScriptableObjectUtility.GetFoldFileCount(weaponFilePath);

        WeaponDef wea = ScriptableObjectUtility.CreateAsset<WeaponDef>(
            count.ToString(),
            weaponFilePath,
            true
        );
        wea.CommonProperty.ID = count;
        wea.CommonProperty.Name = "青铜剑";
        return wea;
    }
    private readonly GUIContent guiContent_ID = new GUIContent("ID", "武器的唯一标识符");
    private readonly GUIContent guiContent_Name = new GUIContent("武器名", "武器的名称");
    private readonly GUIContent guiContent_Desc = new GUIContent("武器描述", "武器的详细描述介绍");
    private readonly GUIContent guiContent_CareerEffect = new GUIContent("职业特效", "对哪些职业造成更多的伤害");
    private readonly GUIContent guiContent_SuperEffect = new GUIContent("超级特效", "具备哪些特殊的效果");
    private readonly GUIContent guiContent_DedicatedCharacter = new GUIContent("人物专用", "该武器只有哪个角色可以使用,其他人均不可使用");
    private readonly GUIContent guiContent_ImportantWeapon = new GUIContent("神器", "是否是很强大的特殊武器");
    private readonly GUIContent guiContent_NoExchange = new GUIContent("不可交换", "该武器不可以和其他人进行交换操作");
    WeaponDef wea;
    public override void OnInspectorGUI()
    {
        //在最开始写Label会显示在最上面
        Rect blockLabelRect = new Rect(45, 5, 120, 16);
        EditorGUI.LabelField(blockLabelRect, new GUIContent("武器"));

        wea.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, wea.CommonProperty.ID);
        wea.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, wea.CommonProperty.Name);
        wea.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, wea.CommonProperty.Description);
        wea.WeaponType = (EnumWeaponType)EditorGUILayout.EnumPopup("武器类型", wea.WeaponType);
        wea.Icon = (Sprite)EditorGUILayout.ObjectField("图标", wea.Icon, typeof(Sprite), false);
        wea.SinglePrice = EditorGUILayout.IntField("单价", wea.SinglePrice);
        wea.UseNumber = EditorGUILayout.IntField("使用次数", wea.UseNumber);
        //根据枚举类型决定该以何种形式呈现
        foldout_Range = EditorGUILayout.Foldout(foldout_Range, "武器攻击范围");
        if (foldout_Range)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            wea.RangeType.RangeType = (EnumWeaponRangeType)EditorGUILayout.EnumPopup("攻击范围类型", wea.RangeType.RangeType);
            wea.RangeType.MinSelectRange = EditorGUILayout.IntField("最小选择距离", wea.RangeType.MinSelectRange);
            wea.RangeType.MaxSelectRange = EditorGUILayout.IntField("最大选择距离", wea.RangeType.MaxSelectRange);
            wea.RangeType.MinEffectRange = EditorGUILayout.IntField("最小生效距离", wea.RangeType.MinEffectRange);
            wea.RangeType.MaxEffectRange = EditorGUILayout.IntField("最大生效距离", wea.RangeType.MaxEffectRange);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        //////////
        wea.Weight = EditorGUILayout.IntField("重量", wea.Weight);
        wea.Power = EditorGUILayout.IntField("威力", wea.Power);
        wea.Hit = EditorGUILayout.IntField("命中率", wea.Hit);
        wea.Crit = EditorGUILayout.IntField("必杀率", wea.Crit); wea.DedicatedCharacter = EditorGUILayout.IntField(guiContent_DedicatedCharacter, wea.DedicatedCharacter);

        EditorGUILayout.BeginHorizontal();
        wea.CareerEffect = EditorGUILayout.IntField(guiContent_CareerEffect, wea.CareerEffect);

        EditorGUILayout.EndHorizontal();
        wea.SuperEffect = EditorGUILayout.IntField(guiContent_SuperEffect, wea.SuperEffect);
        wea.AttackEffect = (EnumWeaponAttackEffectType)EditorGUILayout.EnumPopup("特殊攻击效果", wea.AttackEffect);
        wea.ImportantWeapon = EditorGUILayout.Toggle(guiContent_ImportantWeapon, wea.ImportantWeapon);
        wea.NoExchange = EditorGUILayout.Toggle(guiContent_NoExchange, wea.NoExchange);

        //人物额外属性修正foldout = EditorGUILayout.Foldout(foldout, "武器类型");
        foldout_AdditionalAttribute = EditorGUILayout.Foldout(foldout_AdditionalAttribute, "属性修正");
        if (foldout_AdditionalAttribute)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            wea.AdditionalAttribute.HP = EditorGUILayout.IntField("HP", wea.AdditionalAttribute.HP);
            wea.AdditionalAttribute.PhysicalPower = EditorGUILayout.IntField("物理攻击", wea.AdditionalAttribute.PhysicalPower);
            wea.AdditionalAttribute.MagicalPower = EditorGUILayout.IntField("魔法攻击", wea.AdditionalAttribute.MagicalPower);
            wea.AdditionalAttribute.Skill = EditorGUILayout.IntField("技术", wea.AdditionalAttribute.Skill);
            wea.AdditionalAttribute.Speed = EditorGUILayout.IntField("速度", wea.AdditionalAttribute.Speed);
            wea.AdditionalAttribute.Lucky = EditorGUILayout.IntField("幸运", wea.AdditionalAttribute.Lucky);
            wea.AdditionalAttribute.PhysicalDefense = EditorGUILayout.IntField("物理防御", wea.AdditionalAttribute.PhysicalDefense);
            wea.AdditionalAttribute.MagicalDefense = EditorGUILayout.IntField("魔法防御", wea.AdditionalAttribute.MagicalDefense);
            wea.AdditionalAttribute.Movement = EditorGUILayout.IntField("移动", wea.AdditionalAttribute.Movement);

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

            wea.AdditionalAttributeGrow.HP = EditorGUILayout.IntField("HP成长率", wea.AdditionalAttributeGrow.HP);
            wea.AdditionalAttributeGrow.PhysicalPower = EditorGUILayout.IntField("物理攻击成长率", wea.AdditionalAttributeGrow.PhysicalPower);
            wea.AdditionalAttributeGrow.MagicalPower = EditorGUILayout.IntField("魔法攻击成长率", wea.AdditionalAttributeGrow.MagicalPower);
            wea.AdditionalAttributeGrow.Skill = EditorGUILayout.IntField("技术成长率", wea.AdditionalAttributeGrow.Skill);
            wea.AdditionalAttributeGrow.Speed = EditorGUILayout.IntField("速度成长率", wea.AdditionalAttributeGrow.Speed);
            wea.AdditionalAttributeGrow.Lucky = EditorGUILayout.IntField("幸运成长率", wea.AdditionalAttributeGrow.Lucky);
            wea.AdditionalAttributeGrow.PhysicalDefense = EditorGUILayout.IntField("物理防御成长率", wea.AdditionalAttributeGrow.PhysicalDefense);
            wea.AdditionalAttributeGrow.MagicalDefense = EditorGUILayout.IntField("魔法防御成长率", wea.AdditionalAttributeGrow.MagicalDefense);
            wea.AdditionalAttributeGrow.Movement = EditorGUILayout.IntField("移动成长率", wea.AdditionalAttributeGrow.Movement);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
    CareerMultiSelectionPopWindow careerPopWindow;
    public virtual void OnEnable()
    {
        wea = target as WeaponDef;
        if (careerPopWindow == null)
            careerPopWindow = new CareerMultiSelectionPopWindow();
    }
}
