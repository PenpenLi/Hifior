using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class ResourceManager
{
    private const string ASSET_PROPS = "rpgdata/item/props";
    private const string ASSET_WEAPON = "rpgdata/item/weapon";
    private const string ASSET_MAP = "rpgdata/map";
    private const string ASSET_CHAPTERSETTING = "rpgdata/chaptersetting";
    private const string ASSET_PASSIVESKILL = "rpgdata/passiveskill";
    private const string ASSET_PLAYER = "rpgdata/character/player";
    private const string ASSET_ENEMY = "rpgdata/character/enemy";
    private const string ASSET_PREFAB_PLAYER = "prefab/player";
    private const string ASSET_PREFAB_ENEMY = "prefab/enemy";
    private const string ASSET_TALKBACKGROUND = "talkbackground";
    private static AssetBundle playerPrefabBundle;
    private static AssetBundle enemyPrefabBundle;
    private static Dictionary<int, WeaponDef> weaponDefTable;
    private static Dictionary<int, PropsDef> propsDefTable;
    private static Dictionary<int, PlayerDef> playerDefTable;
    private static Dictionary<int, EnemyDef> enemyDefTable;

    private static MapTileDef mapDef;
    private static Dictionary<int, PassiveSkillDef> passiveSkillDefTable;
    public static PlayerDef GetPlayerDef(int ID)
    {
        //需加载下Prefab的Bundle，否则取不到模型
        if (playerPrefabBundle == null)
            playerPrefabBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, ASSET_PREFAB_PLAYER));
        return GetDef(playerDefTable, ASSET_PLAYER, ID);
    }
    public static Sprite GetTalkBackground(int ID)
    {
        return UGameInstance.LoadAssetFromBundle<Sprite>(Path.Combine(Application.streamingAssetsPath, ASSET_TALKBACKGROUND), ID.ToString());
    }
    public static EnemyDef GetEnemyDef(int ID)
    {
        if (enemyPrefabBundle == null)
            enemyPrefabBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, ASSET_PREFAB_ENEMY));
        return GetDef(enemyDefTable, ASSET_ENEMY, ID);
    }
    public static void UnLoadAllPrefabBundle()
    {
        if (playerPrefabBundle != null)
        {
            playerPrefabBundle.Unload(true);
            playerPrefabBundle = null;
        }
        if (enemyPrefabBundle != null)
        {
            enemyPrefabBundle.Unload(true);
            enemyPrefabBundle = null;
        }
    }
    public static string GetChapterName(int ChapterIndex)
    {
        ChapterSettingDef def = UGameInstance.LoadAssetFromBundle<ChapterSettingDef>(Path.Combine(Application.streamingAssetsPath, ASSET_CHAPTERSETTING), ChapterIndex.ToString());
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
    public static MapTileDef GetMapDef()
    {
        if (mapDef == null)
        {
            mapDef = UGameInstance.LoadAssetFromBundle<MapTileDef>(Path.Combine(Application.streamingAssetsPath, ASSET_MAP), "TileProperty");
        }
        return mapDef;
    }
    public static PassiveSkillDef GetPassiveSkillDef(int ID)
    {
        return GetDef<PassiveSkillDef>(passiveSkillDefTable, ASSET_PASSIVESKILL, ID);
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
            TargetDictionary.Add(ID, UGameInstance.LoadAssetFromBundle<T>(Path.Combine(Application.streamingAssetsPath, AssetBundleURL), ID.ToString()));
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
