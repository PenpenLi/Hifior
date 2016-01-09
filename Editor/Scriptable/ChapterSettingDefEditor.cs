using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
namespace RPGEditor
{
    [CustomEditor(typeof(ChapterSettingDef))]
    public class ChapterSettingDefEditor : Editor
    {
        public static string[] bgmType = new string[] { "我方行动", "敌方行动", "同盟军行动", "我军战斗", "敌军战斗", "同盟军战斗", "出击准备" };
        private static bool foldout_BGM = true;
        ChapterSettingDef chapter;
        private static int forceInvolveCount = 0;
        private static int weaponRoomCount = 0;
        private static int propRoomCount = 0;

        public const string DIRECTORY_PATH = "Assets/RPG Data/Chapter Setting";

        [MenuItem("RPGEditor/Create ChapterSetting", false)]
        public static ChapterSettingDef CreateWeapon()
        {
            int count = ScriptableObjectUtility.GetFoldFileCount(DIRECTORY_PATH);

            ChapterSettingDef chap = ScriptableObjectUtility.CreateAsset<ChapterSettingDef>(
                count.ToString(),
                DIRECTORY_PATH,
                true
            );
            chap.CommonProperty.ID = count;
            return chap;
        }

        public void OnEnable()
        {
            chapter = target as ChapterSettingDef;
            if (chapter.ForceInvolve == null)
                chapter.ForceInvolve = new List<int>();
            forceInvolveCount = chapter.ForceInvolve.Count;
            weaponRoomCount = chapter.WeaponRoom.Count;
            propRoomCount = chapter.PropRoom.Count;

            if (chapter.BGMSetting.Count != bgmType.Length)
            {
                chapter.BGMSetting.Clear();
                chapter.BGMSetting.AddRange(new int[bgmType.Length]);
            }
        }

        private readonly GUIContent guiContent_ID = new GUIContent("ID", "章节的唯一标识符");
        private readonly GUIContent guiContent_Name = new GUIContent("章节名", "章节的名称");
        private readonly GUIContent guiContent_Desc = new GUIContent("章节描述", "章节的详细描述介绍");
        public override void OnInspectorGUI()
        {
            Rect blockLabelRect = new Rect(120, 5, 120, 55);
            EditorGUI.LabelField(blockLabelRect, new GUIContent("章节"), RPGEditorGUI.Head1Style);

            if (chapter.Icon != null)
                EditorGUI.DrawPreviewTexture(new Rect(5, 5, 37, 37), chapter.Icon.texture);

            chapter.CommonProperty.ID = EditorGUILayout.IntField(guiContent_ID, chapter.CommonProperty.ID);
            chapter.CommonProperty.Name = EditorGUILayout.TextField(guiContent_Name, chapter.CommonProperty.Name);
            chapter.CommonProperty.Description = EditorGUILayout.TextField(guiContent_Desc, chapter.CommonProperty.Description);

            chapter.Icon = (Sprite)EditorGUILayout.ObjectField("图标", chapter.Icon, typeof(Sprite), false);

            chapter.MaxPlayerCount = EditorGUILayout.IntSlider("最大出场人数", chapter.MaxPlayerCount, 1, 20);
            string[] displayCharacterNames = RPGData.CharacterNameList.ToArray();
            int[] valueCharacterNames = EnumTables.GetSequentialArray(RPGData.CharacterNameList.Count);
            RPGEditorGUI.DynamicArrayView(ref forceInvolveCount, ref chapter.ForceInvolve, "强制出场人数", "强制出场人物", displayCharacterNames, valueCharacterNames);

            chapter.Weather = (EnumWeather)EditorGUILayout.EnumPopup("默认天气", chapter.Weather);
            chapter.Preparation = EditorGUILayout.Toggle("是否有准备画面", chapter.Preparation);

            WinConditionInspectorGUI(chapter.WinCondition);
            FailConditionInspectorGUI(chapter.FailCondition);

            foldout_BGM = EditorGUILayout.Foldout(foldout_BGM, "BGM设定");
            if (foldout_BGM)
            {
                for (int i = 0; i < bgmType.Length; i++)
                {
                    chapter.BGMSetting[i] = EditorGUILayout.IntPopup(bgmType[i], chapter.BGMSetting[i], AudioPreviewWindow.bgmNameList.ToArray(), EnumTables.GetSequentialArray(AudioPreviewWindow.bgmNameList.Count));
                }
            }
            RPGEditorGUI.DynamicArrayView(ref weaponRoomCount, ref chapter.WeaponRoom, "武器屋", "武器", RPGData.WeaponNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.WeaponNameList.Count));
            RPGEditorGUI.DynamicArrayView(ref propRoomCount, ref chapter.PropRoom, "道具屋", "道具", RPGData.PropNameList.ToArray(), EnumTables.GetSequentialArray(RPGData.PropNameList.Count));

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        public static void WinConditionInspectorGUI(WinCondition win)
        {
            win.Condition = EditorGUILayout.MaskField("胜利条件", win.Condition, System.Enum.GetNames(typeof(EnumWinCondition)));
            if (win.Condition == 0)
                win.Condition = 1;

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            if (EnumTables.MaskFieldIdentify(win.Condition, (int)EnumWinCondition.击败指定Boss))
            {
                win.BossID = EditorGUILayout.IntField("Boss ID", win.BossID);
            }
            if (EnumTables.MaskFieldIdentify(win.Condition, (int)EnumWinCondition.压制指定城池))
            {
                win.CityID = EditorGUILayout.IntField("城池 ID", win.CityID);
            }
            if (EnumTables.MaskFieldIdentify(win.Condition, (int)EnumWinCondition.回合坚持))
            {
                win.Round = EditorGUILayout.IntSlider("回合数", win.Round, 5, 50);
            }
            if (EnumTables.MaskFieldIdentify(win.Condition, (int)EnumWinCondition.领主地点撤离))
            {
                EditorGUILayout.BeginHorizontal();
                win.WinX = EditorGUILayout.IntField("X", win.WinX);
                win.WinY = EditorGUILayout.IntField("Y", win.WinY);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        public static void FailConditionInspectorGUI(FailCondition fail)
        {
            fail.Condition = EditorGUILayout.MaskField("失败条件", fail.Condition, System.Enum.GetNames(typeof(EnumFailCondition)));

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            if (EnumTables.MaskFieldIdentify(fail.Condition, (int)EnumFailCondition.人物死亡))
            {
                fail.PlayerID = EditorGUILayout.IntField("人物 ID", fail.PlayerID);
            }
            if (EnumTables.MaskFieldIdentify(fail.Condition, (int)EnumFailCondition.城池被夺))
            {
                fail.CityID = EditorGUILayout.IntField("城池 ID", fail.CityID);
            }
            if (EnumTables.MaskFieldIdentify(fail.Condition, (int)EnumFailCondition.回合达到))
            {
                fail.Round = EditorGUILayout.IntField("回合数", fail.Round);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}