using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(CharacterDef),true)]
    public class CharacterDefEditor : Editor
    {
        private static bool foldout_Attribute = true;

        private readonly GUIContent guiContent_ID = new GUIContent("ID", "人物的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("人物名称", "人物的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("人物描述", "人物的详细描述介绍");

        CharacterDef character;
        public override void OnInspectorGUI()
        {
            Rect blockLabelRect = new Rect(60, 5, 120, 55);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("人物"), RPGEditorGUI.Head1Style);

            if (character.Portrait != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), character.Portrait.texture);
            character.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, character.CommonProperty.ID);
            character.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, character.CommonProperty.Name);
            character.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, character.CommonProperty.Description);

            character.Portrait = (Sprite)EditorGUILayout.ObjectField("图标", character.Portrait, typeof(Sprite), false);
            character.BattleModel = EditorGUILayout.ObjectField("人物模型", character.BattleModel, typeof(GameObject), true) as GameObject;
            character.CharacterImportance = (EnumCharacterImportance)EditorGUILayout.EnumPopup("重要性", character.CharacterImportance);
            character.Career = EditorGUILayout.IntPopup("职业", character.Career, RPGData.CareerNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.CareerNameList.Count));
            character.DefaultLevel = EditorGUILayout.IntSlider("初始等级", character.DefaultLevel, 1, 40);

            foldout_Attribute = EditorGUILayout.Foldout(foldout_Attribute, "初始属性");
            if (foldout_Attribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                character.DefaultAttribute.HP = EditorGUILayout.IntSlider("HP", character.DefaultAttribute.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                character.DefaultAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", character.DefaultAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", character.DefaultAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Skill = EditorGUILayout.IntSlider("技术", character.DefaultAttribute.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Speed = EditorGUILayout.IntSlider("速度", character.DefaultAttribute.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Lucky = EditorGUILayout.IntSlider("幸运", character.DefaultAttribute.Lucky, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", character.DefaultAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", character.DefaultAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Movement = EditorGUILayout.IntSlider("移动", character.DefaultAttribute.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        public void InitTarget(CharacterDef def)
        {
            character = def;
        }
    }
}
