using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace RPGEditor
{
    public class MiscCoefficientSettingWindow : EditorWindow
    {
        public const string MISCSETTING_FILEPATH = "Assets/RPG Data/Misc";
        public const string MISCSETTING_DATANAME = "MiscCoefficientSetting";

        public static MiscCoefficientSetting MiscSetting;
        [MenuItem("RPGEditor/Misc/Set Miscellaneous Coefficient", false,50)]
        public static void OpenEditor()
        {
            MiscSetting = CreateTileData();
            MiscCoefficientSettingWindow mapEditor = EditorWindow.GetWindow<MiscCoefficientSettingWindow>();
            mapEditor.ShowPopup();
        }
        public static MiscCoefficientSetting CreateTileData()
        {
            string absolutePath = MISCSETTING_FILEPATH + "/" + MISCSETTING_DATANAME + ".asset";
            if (!ScriptableObjectUtility.FileExists(absolutePath))
            {
                MiscCoefficientSetting misc = ScriptableObjectUtility.CreateAsset<MiscCoefficientSetting>(
                    MISCSETTING_DATANAME,
                    MISCSETTING_FILEPATH,
                    true
                );
                return misc;
            }
            return AssetDatabase.LoadAssetAtPath(absolutePath, typeof(MiscCoefficientSetting)) as MiscCoefficientSetting;
        }
        void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            MiscSetting.MoneyUpperLimit = EditorGUILayout.IntField("金钱上限", MiscSetting.MoneyUpperLimit);

            MiscSetting.PassiveSkillHoldUpperLimit = EditorGUILayout.IntSlider("被动技能持有上限", MiscSetting.PassiveSkillHoldUpperLimit, 5, 10);

            MiscSetting.InitiativeSkillHoldUpperLimit = EditorGUILayout.IntSlider("主动技能持有上限", MiscSetting.InitiativeSkillHoldUpperLimit, 5, 10);

            MiscSetting.WeaponHoldUpperLimit = EditorGUILayout.IntSlider("武器持有上限", MiscSetting.WeaponHoldUpperLimit, 5, 10);

            MiscSetting.FerryHoldUpperLimit = EditorGUILayout.IntSlider("运输队物品持有上限", MiscSetting.FerryHoldUpperLimit, 50, 200);

            MiscSetting.CareerTransferLevel = EditorGUILayout.IntSlider("低阶职业转职临界点", MiscSetting.CareerTransferLevel, 10, 30);

            MiscSetting.LevelUpperLimit = EditorGUILayout.IntSlider("最大等级上限", MiscSetting.LevelUpperLimit, 30, 50);

            MiscSetting.RepeatAttackSpeedGap = EditorGUILayout.IntSlider("攻击第二次的速度差", MiscSetting.RepeatAttackSpeedGap, 1, 10);

            MiscSetting.SelfRecoveryCoefficient = EditorGUILayout.IntSlider("自动恢复的系数", MiscSetting.SelfRecoveryCoefficient, 0, 50);

            MiscSetting.BossAdditionExp = EditorGUILayout.IntSlider("Boss击败额外经验值", MiscSetting.BossAdditionExp, 30, 100);

            MiscSetting.KillAdditionExp = EditorGUILayout.IntSlider("普通小兵击败额外经验值", MiscSetting.KillAdditionExp, 10, 50); ;

            EditorGUILayout.EndVertical();
        }
    }
}
