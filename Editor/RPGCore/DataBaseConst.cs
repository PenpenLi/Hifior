using System;

namespace RPGEditor
{
    public static class DataBaseConst
    {
        public const string ScriptableObjectBasePath = "Assets/Resources/rpgdata/";

        public const string DataBase_Props_Folder = DataBaseConst.ScriptableObjectBasePath + "item/props";

        public const string DataBase_Weapon_Folder = DataBaseConst.ScriptableObjectBasePath + "item/weapon";

        public const string DataBase_Career_Folder = DataBaseConst.ScriptableObjectBasePath + "career";

        public const string DataBase_CombatBuff_Folder = DataBaseConst.ScriptableObjectBasePath + "combat";

        public const string DataBase_Player_Folder = DataBaseConst.ScriptableObjectBasePath + "character/player";
        public const string DataBase_Enemy_Folder = DataBaseConst.ScriptableObjectBasePath + "character/enemy";

        public const string DataBase_ChapterSetting_Folder = DataBaseConst.ScriptableObjectBasePath + "chapter";
        public const string DataBase_PassiveSkill_Folder = DataBaseConst.ScriptableObjectBasePath + "passiveskill";

        public const string DataBase_GameSetting_File = DataBaseConst.ScriptableObjectBasePath + "misc/MiscCoefficientSetting.asset";
    }
}
