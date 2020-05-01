using UnityEngine;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(CombatBufferDef))]
    public class CombatBuffDefEditor : Editor
    {

        public const string DIRECTORY_PATH = DataBaseConst.DataBase_CombatBuff_Folder;
        [MenuItem("RPGEditor/Create Combat Buff", false, 0)]
        public static CombatBufferDef CreateCombatBuff()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            CombatBufferDef buff = ScriptableObjectUtility.CreateAsset<CombatBufferDef>(
                count.ToString(),
                DIRECTORY_PATH,
                true
            );
            buff.CommonProperty.ID = count;
            buff.CommonProperty.Name = "";
            return buff;
        }
        private readonly GUIContent guiContent_ID = new GUIContent("ID", "Buff的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("Buff名称", "人物的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("Buff描述", "Buff的详细描述介绍");

        CombatBufferDef buff;
        public override void OnInspectorGUI()
        {
            Rect blockLabelRect = new Rect(60, 5, 120, 55);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("Buff"), RPGEditorGUI.Head1Style);

            if (buff.Icon != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), buff.Icon.texture);

            buff.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, buff.CommonProperty.ID);
            buff.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, buff.CommonProperty.Name);
            buff.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, buff.CommonProperty.Description);

            buff.Icon = (Sprite)EditorGUILayout.ObjectField("图标", buff.Icon, typeof(Sprite), false);
            
                buff.DisappearRound = EditorGUILayout.IntSlider("消失回合数", buff.DisappearRound, 1, 10);

            buff.DisapperOnExitBattle = EditorGUILayout.Toggle("回合结束后buff消失", buff.DisapperOnExitBattle);

            buff.Effect = (EnumCombatBuffEffect)EditorGUILayout.EnumPopup("buff效果", buff.Effect);
            if (buff.Effect == EnumCombatBuffEffect.人物属性固定改变 || buff.Effect == EnumCombatBuffEffect.人物属性百分比改变)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                buff.AttributeChange.HP = EditorGUILayout.IntSlider("HP", buff.AttributeChange.HP, 0, RPGEditorGlobal.MAX_ATTRIBUTE_HP);
                buff.AttributeChange.PhysicalPower = EditorGUILayout.IntSlider("物理攻击", buff.AttributeChange.PhysicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.MagicalPower = EditorGUILayout.IntSlider("魔法攻击", buff.AttributeChange.MagicalPower, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.Skill = EditorGUILayout.IntSlider("技术", buff.AttributeChange.Skill, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.Speed = EditorGUILayout.IntSlider("速度", buff.AttributeChange.Speed, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.Intel = EditorGUILayout.IntSlider("智力", buff.AttributeChange.Intel, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.PhysicalDefense = EditorGUILayout.IntSlider("物理防御", buff.AttributeChange.PhysicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.MagicalDefense = EditorGUILayout.IntSlider("魔法防御", buff.AttributeChange.MagicalDefense, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.BodySize = EditorGUILayout.IntSlider("体格", buff.AttributeChange.BodySize, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MISC);
                buff.AttributeChange.Movement = EditorGUILayout.IntSlider("移动", buff.AttributeChange.Movement, 0, RPGEditorGlobal.MAX_ATTRIBUTE_MOVEMENT);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            if (buff.Effect == EnumCombatBuffEffect.每回合掉百分比HP)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
                EditorGUILayout.Space();
                buff.AttributeChange.HP = EditorGUILayout.IntSlider("HP百分比", buff.AttributeChange.HP, 10, 50);
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        public void OnEnable()
        {
            buff = target as CombatBufferDef;
        }
    }
}