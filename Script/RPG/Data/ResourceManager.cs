using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
#pragma warning disable 0649
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

    #region Const Data 图片 音乐等不会改变的素材
    public static Sprite GetTalkBackground(int ID)
    {
        return Resources.Load<Sprite>(Path.Combine(ASSET_BG, ID.ToString())); ;
    }
    #endregion
    public static PlayerDef GetPlayerDef(int ID)
    {
        return GetDef(playerDefTable, ASSET_PLAYER, ID);
    }
    public static EnemyDef GetEnemyDef(int ID)
    {
        return GetDef(enemyDefTable, ASSET_ENEMY, ID);
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

    public static ChapterSettingDef GetChapterDef(int ChapterIndex)
    {
        ChapterSettingDef def = Resources.Load<ChapterSettingDef>(Path.Combine(ASSET_CHAPTERSETTING, ChapterIndex.ToString()));
        ChapterSettingDef r = GameObject.Instantiate<ChapterSettingDef>(def);
        if (r.Event == null) Debug.LogError("你需要在ChapterDef" + r.CommonProperty.ID + "中指定Event");
        r.Event.SetChapterDef(r);
        return r;
    }
    public static string GetChapterName(int ChapterIndex)
    {
        ChapterSettingDef def = GetChapterDef(ChapterIndex);
        return def.CommonProperty.Name;
    }
    private static T GetDef<T>(Dictionary<int, T> TargetDictionary, string assetUrl, int ID) where T : ScriptableObject
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
            var res = Resources.Load<T>(Path.Combine(assetUrl, ID.ToString()));
            if (res == null) { Debug.LogError("the res is null:" + assetUrl + " :" + ID); return null; }
            TargetDictionary.Add(ID, GameObject.Instantiate<T>(res));
            return TargetDictionary[ID];
        }
    }
    #region Sequence
    private const string RESOURCE_SEQUENCE_DIR = "rpgdata/chapter/sequence/";
    private const string RESOURCE_SEQUENCE_TREASUREBOX = RESOURCE_SEQUENCE_DIR+ "TreasureBox";
    public static Sequence.Sequence Sequence_TreasureBox;
    public static void LoadSequence()
    {
        var temp = Resources.Load<Sequence.Sequence>(RESOURCE_SEQUENCE_TREASUREBOX);
        Sequence_TreasureBox = GameObject.Instantiate(temp);
    }
    #endregion
    public static void UnloadUnusedResource()
    {
        Resources.UnloadUnusedAssets();
    }
    public static void UnloadAllRPGData()
    {
        weaponDefTable.Clear();
        propsDefTable.Clear();
        careerDefTable.Clear();
        playerDefTable.Clear();
        enemyDefTable.Clear();
    }
    #region Material 
    private const string RESOURCE_APP_MATERIAL_DIR = "App/Material/";
    private const string RESOURCE_APP_MATERIAL_UNIT_ALLY = "UnitAlly";
    private const string RESOURCE_APP_MATERIAL_UNIT_ENEMY = "UnitEnemy";
    private const string RESOURCE_APP_MATERIAL_UNIT_NPC = "UnitNPC";
    private const string RESOURCE_APP_MATERIAL_UNIT_PLAYER = "UnitPlayer";
    private const string RESOURCE_APP_MATERIAL_UNIT_GREY = "UnitGrey";
    private static Dictionary<string, Material> app_material = new Dictionary<string, Material>();
    public static void LoadMaterial()
    {
        var res = Resources.LoadAll<Material>(RESOURCE_APP_MATERIAL_DIR);
        foreach (var v in res)
        {
            app_material.Add(v.name, v);
        }
    }
    public static Material UnitGreyMaterial { get { return app_material[RESOURCE_APP_MATERIAL_UNIT_GREY]; } }
    public static Material UnitPlayerMaterial { get { return app_material[RESOURCE_APP_MATERIAL_UNIT_PLAYER]; } }
    public static Material UnitEnemyMaterial { get { return app_material[RESOURCE_APP_MATERIAL_UNIT_ENEMY]; } }
    public static Material UnitAllyMaterial { get { return app_material[RESOURCE_APP_MATERIAL_UNIT_ALLY]; } }
    public static Material UnitNPCMaterial { get { return app_material[RESOURCE_APP_MATERIAL_UNIT_NPC]; } }
    public static Material GetUnitMaterial(EnumCharacterCamp camp)
    {
        switch (camp)
        {
            case EnumCharacterCamp.Player:return UnitPlayerMaterial;
            case EnumCharacterCamp.Enemy:return UnitEnemyMaterial;
            case EnumCharacterCamp.Ally:return UnitAllyMaterial;
            case EnumCharacterCamp.NPC:return UnitNPCMaterial;
        }
        return UnitPlayerMaterial;
    }
    #endregion
    static ResourceManager()
    {
        LoadMaterial();
    }
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        
    }
}
