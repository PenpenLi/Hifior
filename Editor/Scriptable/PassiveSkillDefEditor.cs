using UnityEngine;
using System.Collections;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(PassiveSkillDef))]
    public class PassiveSkillDefEditor : Editor
    {
        PassiveSkillDef passiveSkill;
        public const string DIRECTORY_PATH = "Assets/RPG Data/Passive Skill";
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "技能的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("技能名称", "技能的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("技能描述", "技能的详细描述介绍");
        [MenuItem("RPGEditor/Create Passive Skill", false, 1)]
        public static PassiveSkillDef CreatePassiveSkill()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            PassiveSkillDef skill = ScriptableObjectUtility.CreateAsset<PassiveSkillDef>(count.ToString(), DIRECTORY_PATH,true );
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
        }
        public void OnEnable()
        {
            passiveSkill = target as PassiveSkillDef;
        }
    }
}
