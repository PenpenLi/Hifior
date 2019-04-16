using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class ResourceManager
{
    private const string ASSET_PROPS = "rpgdata/item/props";
    private const string ASSET_WEAPON = "rpgdata/item/weapon";
    private const string ASSET_MAP = "rpgdata/map";
    private const string ASSET_CHAPTERSETTING = "rpgdata/chapter";
    private const string ASSET_CAREER = "rpgdata/career";
    private const string ASSET_PASSIVESKILL = "rpgdata/passiveskill";
    private const string ASSET_PLAYER = "rpgdata/character/player";
    private const string ASSET_ENEMY = "rpgdata/character/enemy";
    private const string ASSET_BG = "bg";
    private static Dictionary<int, WeaponDef> weaponDefTable;
    private static Dictionary<int, PropsDef> propsDefTable;
    private static Dictionary<int, CareerDef> careerDefTable;
    private static Dictionary<int, PlayerDef> playerDefTable;
    private static Dictionary<int, EnemyDef> enemyDefTable;

    private static MapTileDef mapDef;
    private static Dictionary<int, PassiveSkillDef> passiveSkillDefTable;
    public static PlayerDef GetPlayerDef(int ID)
    {
        return Resources.Load<PlayerDef>(Path.Combine(ASSET_PLAYER, ID.ToString()));
    }
    public static Sprite GetTalkBackground(int ID)
    {
        return Resources.Load<Sprite>(Path.Combine(ASSET_BG, ID.ToString())); ;
    }
    public static EnemyDef GetEnemyDef(int ID)
    {
        return Resources.Load<EnemyDef>(Path.Combine(ASSET_ENEMY, ID.ToString()));
    }
    public static ChapterSettingDef GetChapterDef(int ChapterIndex)
    {
        ChapterSettingDef def = Resources.Load<ChapterSettingDef>(Path.Combine(ASSET_CHAPTERSETTING, ChapterIndex.ToString()));
        def.Event.SetChapterDef(def);
        return def;
    }
    public static string GetChapterName(int ChapterIndex)
    {
        ChapterSettingDef def = GetChapterDef(ChapterIndex);
        return def.CommonProperty.Name;
    }

    public static PropsDef GetPropsDef(int ID)
    {
        return GetDef(propsDefTable, ASSET_PROPS, ID);
    }
    public static WeaponDef GetWeaponDef(int ID)
    {
        return GetDef(weaponDefTable, ASSET_WEAPON, ID);
    }
    public static CareerDef GetCareerDef(int ID)
    {
        return GetDef(careerDefTable, ASSET_CAREER, ID);
    }
    private static T GetDef<T>(Dictionary<int, T> TargetDictionary, string AssetBundleURL, int ID) where T : ScriptableObject
    {
        if (TargetDictionary == null)
        {
            TargetDictionary = new Dictionary<int, T>();
        }
        if (TargetDictionary.ContainsKey(ID))
        {
            return TargetDictionary[ID];
        }
        else
        {
            var res = Resources.Load<T>(Path.Combine(AssetBundleURL, ID.ToString()));
            if (res == null) { Debug.LogError("the res is null" + AssetBundleURL + " :" + ID); return null; }
            TargetDictionary.Add(ID, res);
            return TargetDictionary[ID];
        }
    }

    public static void UnloadAllRPGData()
    {
        weaponDefTable.Clear();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {

    }
}
