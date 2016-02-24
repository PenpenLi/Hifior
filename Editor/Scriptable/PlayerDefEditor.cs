using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(PlayerDef))]
    public class PlayerDefEditor : CharacterDefEditor
    {
        private static int weaponCount;
        private static bool foldout_AttributeGrow = true;
        private static bool foldout_Attribute = true;

        public const string DIRECTORY_PATH = "Assets/RPG Data/Character/Player";
        [MenuItem("RPGEditor/Create Character/Player", false, 0)]
        public static PlayerDef CreateProps()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            PlayerDef character = ScriptableObjectUtility.CreateAsset<PlayerDef>(
                count.ToString(),
                DIRECTORY_PATH,
                true
            );

            character.CommonProperty.ID = count;
            character.CommonProperty.Name = "";
            return character;
        }

        PlayerDef player;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            /*Rect blockLabelRect = new Rect(60, 5, 120, 55);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("人物"), RPGEditorGUI.Head1Style);

            if (player.Portrait != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), player.Portrait.texture);

            player.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, player.CommonProperty.ID);
            player.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, player.CommonProperty.Name);
            player.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, player.CommonProperty.Description);

            player.Portrait = (Sprite)EditorGUILayout.ObjectField("图标", player.Portrait, typeof(Sprite), false);
            player.BattleModel = EditorGUILayout.ObjectField("人物模型", player.BattleModel, typeof(GameObject), true) as GameObject;
            player.CharacterImportance = (EnumCharacterImportance)EditorGUILayout.EnumPopup("重要性", player.CharacterImportance);
            player.Career = EditorGUILayout.IntPopup("职业", player.Career, RPGData.CareerNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.CareerNameList.Count));
            player.DefaultLevel = EditorGUILayout.IntSlider("初始等级", player.DefaultLevel, 1, 40);

            foldout_Attribute = EditorGUILayout.Foldout(foldout_Attribute, "初始属性");
            if (foldout_Attribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                player.DefaultAttribute.HP = EditorGUILayout.IntSlider("HP", player.DefaultAttribute.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                player.DefaultAttribute.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", player.DefaultAttribute.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", player.DefaultAttribute.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.Skill = EditorGUILayout.IntSlider("技术", player.DefaultAttribute.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.Speed = EditorGUILayout.IntSlider("速度", player.DefaultAttribute.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.Lucky = EditorGUILayout.IntSlider("幸运", player.DefaultAttribute.Lucky, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", player.DefaultAttribute.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", player.DefaultAttribute.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttribute.Movement = EditorGUILayout.IntSlider("移动", player.DefaultAttribute.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }*/

            foldout_AttributeGrow = EditorGUILayout.Foldout(foldout_AttributeGrow, "成长率");
            if (foldout_Attribute)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                player.DefaultAttributeGrow.HP = EditorGUILayout.IntSlider("HP", player.DefaultAttributeGrow.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                player.DefaultAttributeGrow.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", player.DefaultAttributeGrow.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", player.DefaultAttributeGrow.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.Skill = EditorGUILayout.IntSlider("技术", player.DefaultAttributeGrow.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.Speed = EditorGUILayout.IntSlider("速度", player.DefaultAttributeGrow.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.Luck = EditorGUILayout.IntSlider("幸运", player.DefaultAttributeGrow.Luck, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", player.DefaultAttributeGrow.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", player.DefaultAttributeGrow.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                player.DefaultAttributeGrow.Movement = EditorGUILayout.IntSlider("移动", player.DefaultAttributeGrow.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            RPGEditorGUI.DynamicArrayView(ref weaponCount, ref player.DefaultWeapons, "初始武器", "武器", RPGData.WeaponNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.WeaponNameList.Count));

            player.DeadSpeech = EditorGUILayout.TextField("战败话语", player.DeadSpeech);
            player.LeaveSpeech = EditorGUILayout.TextField("战败话语", player.LeaveSpeech);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        public void OnEnable()
        {
            player = target as PlayerDef;
            base.InitTarget(player);
            weaponCount = player.DefaultWeapons.Count;
        }
    }
}
