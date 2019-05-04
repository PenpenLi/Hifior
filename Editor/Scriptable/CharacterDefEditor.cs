using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(CharacterDef), true)]
    public class CharacterDefEditor : Editor
    {
        private static bool foldout_Attribute = true;
        public const int TALK_PORTRAIT_COUNT = 4;
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "人物的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("人物名称", "人物的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("人物描述", "人物的详细描述介绍");

        CharacterDef character;
        private void SetTalkPortraitListCount()
        {
            if (character.TalkPortrait == null)
            {
                character.TalkPortrait = new System.Collections.Generic.List<Sprite>();
            }
            int x = TALK_PORTRAIT_COUNT - character.TalkPortrait.Count;
            for (int i = 0; i < x; i++)
            {
                character.TalkPortrait.Add(null);
            }
        }
        public override void OnInspectorGUI()
        {
            SetTalkPortraitListCount();
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

                character.DefaultAttribute.HP = EditorGUILayout.IntSlider("HP", character.DefaultAttribute.HP, 20, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                character.DefaultAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", character.DefaultAttribute.PhysicalPower, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", character.DefaultAttribute.MagicalPower, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Skill = EditorGUILayout.IntSlider("技术", character.DefaultAttribute.Skill, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Speed = EditorGUILayout.IntSlider("速度", character.DefaultAttribute.Speed, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Intel = EditorGUILayout.IntSlider("智力", character.DefaultAttribute.Intel, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", character.DefaultAttribute.PhysicalDefense, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", character.DefaultAttribute.MagicalDefense, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.BodySize = EditorGUILayout.IntSlider("体格", character.DefaultAttribute.BodySize, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                character.DefaultAttribute.Movement = EditorGUILayout.IntSlider("移动", character.DefaultAttribute.Movement, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            for (int i = 0; i < TALK_PORTRAIT_COUNT; i++)
            {
                character.TalkPortrait[i] = (Sprite)EditorGUILayout.ObjectField("对话头像:" + i, character.Portrait, typeof(Sprite), false);
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        public static void CharacterAttributeInspector(ref CharacterAttribute Attribute, ref bool foldout_Attribute)
        {
            foldout_Attribute = EditorGUILayout.Foldout(foldout_Attribute, "初始属性");
            if (foldout_Attribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                Attribute.HP = EditorGUILayout.IntSlider("HP", Attribute.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                Attribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", Attribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", Attribute.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.Skill = EditorGUILayout.IntSlider("技术", Attribute.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.Speed = EditorGUILayout.IntSlider("速度", Attribute.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.Intel = EditorGUILayout.IntSlider("智力", Attribute.Intel, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", Attribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", Attribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.BodySize = EditorGUILayout.IntSlider("体格", Attribute.BodySize, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Attribute.Movement = EditorGUILayout.IntSlider("移动", Attribute.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

        }
        public void InitTarget(CharacterDef def)
        {
            character = def;
        }
    }



    public abstract class CharacterEditorProp<T> : EditorProp<T> where T:CharacterDef
    {
        public const int TALK_PORTRAIT_COUNT = 4;
        private static bool foldout_Attribute = true;

        public override string ListName(int index)
        {
            string Name = scriptableObjects[index].CommonProperty.Name;
            if (string.IsNullOrEmpty(Name.Trim()))
                return base.NO_NAME;
            return Name;
        }

        public override void OnGUI(T Data)
        {
            if (Data.TalkPortrait == null)
            {
                Data.TalkPortrait = new System.Collections.Generic.List<Sprite>();
            }
            int x = TALK_PORTRAIT_COUNT - Data.TalkPortrait.Count;
            for (int i = 0; i < x; i++)
            {
                Data.TalkPortrait.Add(null);
            }
            Data.CommonProperty.ID = EditorGUILayout.IntField("ID", Data.CommonProperty.ID);
            Data.CommonProperty.Name = EditorGUILayout.TextField("Name", Data.CommonProperty.Name);
            Data.CommonProperty.Description = EditorGUILayout.TextField("描述", Data.CommonProperty.Description);

            Data.Portrait = (Sprite)EditorGUILayout.ObjectField("图标", Data.Portrait, typeof(Sprite), false);
            Data.BattleModel = EditorGUILayout.ObjectField("人物模型", Data.BattleModel, typeof(GameObject), true) as GameObject;
            Data.CharacterImportance = (EnumCharacterImportance)EditorGUILayout.EnumPopup("重要性", Data.CharacterImportance);
            Data.Career = EditorGUILayout.IntPopup("职业", Data.Career,DataBaseWindow.CareerNameList.ToArray(), EnumTables.GetSequentialArray(DataBaseWindow.CareerNameList.Count));
            Data.DefaultLevel = EditorGUILayout.IntSlider("初始等级", Data.DefaultLevel, 1, 40);

            foldout_Attribute = EditorGUILayout.Foldout(foldout_Attribute, "初始属性");
            if (foldout_Attribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                Data.DefaultAttribute.HP = EditorGUILayout.IntSlider("HP", Data.DefaultAttribute.HP, 20, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                Data.DefaultAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", Data.DefaultAttribute.PhysicalPower, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", Data.DefaultAttribute.MagicalPower, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.Skill = EditorGUILayout.IntSlider("技术", Data.DefaultAttribute.Skill, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.Speed = EditorGUILayout.IntSlider("速度", Data.DefaultAttribute.Speed, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.Intel = EditorGUILayout.IntSlider("智力", Data.DefaultAttribute.Intel, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", Data.DefaultAttribute.PhysicalDefense, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", Data.DefaultAttribute.MagicalDefense, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.BodySize = EditorGUILayout.IntSlider("体格", Data.DefaultAttribute.BodySize, 1, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                Data.DefaultAttribute.Movement = EditorGUILayout.IntSlider("移动", Data.DefaultAttribute.Movement, 4, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            for (int i = 0; i < TALK_PORTRAIT_COUNT; i++)
            {
                Data.TalkPortrait[i] = (Sprite)EditorGUILayout.ObjectField("对话头像:" + i, Data.Portrait, typeof(Sprite), false);
            }
        }
    }
}
