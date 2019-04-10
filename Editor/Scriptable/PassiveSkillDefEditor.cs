using UnityEngine;
using System.Collections;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(PassiveSkillDef))]
    public class PassiveSkillDefEditor : Editor
    {
        PassiveSkillDef passiveSkill;
        public const string DIRECTORY_PATH =DataBaseConst.DataBase_PassiveSkill_Folder;
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "技能的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("技能名称", "技能的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("技能描述", "技能的详细描述介绍");
        [MenuItem("RPGEditor/Create Passive Skill", false, 1)]
        public static PassiveSkillDef CreatePassiveSkill()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            PassiveSkillDef skill = ScriptableObjectUtility.CreateAsset<PassiveSkillDef>(count.ToString(), DIRECTORY_PATH, true);
            skill.CommonProperty.ID = count;
            skill.CommonProperty.Name = "";
            return skill;
        }
        public override void OnInspectorGUI()
        {
            Rect blockLabelRect = new Rect(60, 5, 120, 55);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("技能"), RPGEditorGUI.Head1Style);

            if (passiveSkill.Icon != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), passiveSkill.Icon.texture);
            passiveSkill.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, passiveSkill.CommonProperty.ID);
            passiveSkill.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, passiveSkill.CommonProperty.Name);
            passiveSkill.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, passiveSkill.CommonProperty.Description);

            passiveSkill.Icon = (Sprite)EditorGUILayout.ObjectField("图标", passiveSkill.Icon, typeof(Sprite), false);
            passiveSkill.EventTrigger = (EnumBuffSkillTrigger)EditorGUILayout.EnumPopup("事件触发点", passiveSkill.EventTrigger);
            passiveSkill.Effect = (EnumPassiveSkillEffect)EditorGUILayout.EnumPopup("被动技能效果", passiveSkill.Effect);
            if (passiveSkill.Effect == EnumPassiveSkillEffect.人物属性固定改变 || passiveSkill.Effect == EnumPassiveSkillEffect.人物属性百分比改变)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                passiveSkill.AttributeChange.HP = EditorGUILayout.IntSlider("HP", passiveSkill.AttributeChange.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                passiveSkill.AttributeChange.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", passiveSkill.AttributeChange.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", passiveSkill.AttributeChange.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.Skill = EditorGUILayout.IntSlider("技术", passiveSkill.AttributeChange.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.Speed = EditorGUILayout.IntSlider("速度", passiveSkill.AttributeChange.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.Luck = EditorGUILayout.IntSlider("幸运", passiveSkill.AttributeChange.Luck, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", passiveSkill.AttributeChange.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", passiveSkill.AttributeChange.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                passiveSkill.AttributeChange.Movement = EditorGUILayout.IntSlider("移动", passiveSkill.AttributeChange.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            if (passiveSkill.Effect == EnumPassiveSkillEffect.回复百分比HP)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                passiveSkill.AttributeChange.HP = EditorGUILayout.IntSlider("HP百分比", passiveSkill.AttributeChange.HP, 10, 50);
                EditorGUILayout.EndHorizontal();
            }
        }
        public void OnEnable()
        {
            passiveSkill = target as PassiveSkillDef;
        }
    }
}
