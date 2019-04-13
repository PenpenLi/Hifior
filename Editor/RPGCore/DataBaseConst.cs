using System;

namespace RPGEditor
{
    public static class DataBaseConst
    {
        public const string ScriptableObjectBasePath = "Assets/Resources/rpgdata/";

        public const string DataBase_Props_Folder = DataBaseConst.ScriptableObjectBasePath + "Item/Props";

        public const string DataBase_Weapon_Folder = DataBaseConst.ScriptableObjectBasePath + "Item/Weapon";

        public const string DataBase_Career_Folder = DataBaseConst.ScriptableObjectBasePath + "Career";

        public const string DataBase_CombatBuff_Folder = DataBaseConst.ScriptableObjectBasePath + "Combat Buffer";

        public const string DataBase_Player_Folder = DataBaseConst.ScriptableObjectBasePath + "Character/Player";
        public const string DataBase_Enemy_Folder = DataBaseConst.ScriptableObjectBasePath + "Character/Enemy";

        public const string DataBase_ChapterSetting_Folder = DataBaseConst.ScriptableObjectBasePath + "Chapter Setting";
        public const string DataBase_PassiveSkill_Folder = DataBaseConst.ScriptableObjectBasePath + "Passive Skill";

        public const string DataBase_GameSetting_File = DataBaseConst.ScriptableObjectBasePath + "Misc/MiscCoefficientSetting.asset";
    }
}
