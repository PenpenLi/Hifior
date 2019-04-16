using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理章节数据和检测
/// 人员加载配置，各种事件触发等
/// </summary>
public class ChapterManager : ManagerBase
{
    public SLGChapter Event { get { return ChapterDef.Event; } }
    public ChapterSettingDef ChapterDef;
    private List<RPGPlayer> players;
    private List<RPGEnemy> enemies;
    private List<CharacterLogic> playerLogic;
    private Warehouse warehouse;

    public int Condition;
    public int BossID;
    public int CityID;
    public int Round;
    public int WinX, WinY;
    public ChapterManager()
    {
        players = new List<RPGPlayer>();
        enemies = new List<RPGEnemy>();
        playerLogic = new List<CharacterLogic>();
        warehouse = new Warehouse();
    }
    /// <summary>
    /// 清除战场数据
    /// </summary>
    public void ClearBattle()
    {
        players.Clear();
        enemies.Clear();
    }

    /// <summary>
    /// 仅添加数据
    /// </summary>
    public void AddPlayerLogic(PlayerDef def)
    {
        CharacterLogic logic = new CharacterLogic(def);
        playerLogic.Add(logic);
    }

    /// <summary>
    /// 添加到战场
    /// </summary>
    public void AddPlayerToBattle(RPGCharacter ch)
    {
        UnityEngine.Assertions.Assert.AreEqual(ch.GetCamp(), EnumCharacterCamp.Player);
        RPGPlayer player = ch as RPGPlayer;
        players.Add(player);
        playerLogic.Add(player.Logic);
    }
    public void AddEnemyToBattle()
    {

    }
    public RPGCharacter GetCharacterFromCoord(Vector2Int tilePos)
    {
        foreach (var v in players)
            if (v.GetTileCoord() == tilePos) return v;
        foreach (var v in enemies)
            if (v.GetTileCoord() == tilePos) return v;
        return null;
    }
    public bool HasCharacterFromCoord(Vector2Int tilePos)
    {
        return GetCharacterFromCoord(tilePos) != null;
    }
    #region GameRecord

    private static GameRecord record = new GameRecord();
    /// <summary>
    /// 记录战场情况
    /// </summary>
    public void SaveBattle()
    {

    }
    /// <summary>
    /// 仅章节结束后的数据
    /// </summary>
    public void SaveChapterData(int slot)
    {
        List<CharacterInfo> playerInfos = new List<CharacterInfo>();
        foreach (var v in players)
        {
            playerInfos.Add(v.Logic.Info);
        }
        if (record == null) Debug.LogError("GameRecord is null");
        if (ChapterDef == null) Debug.LogError("ChapterDef is null");
        record.SaveChapter(slot, ChapterDef.TeamIndex, true, ChapterDef.CommonProperty.ID, warehouse, playerInfos);
        record.SaveBattle(ChapterDef.TeamIndex, ChapterDef.CommonProperty.ID, warehouse, playerInfos, Event.EventInfo);
    }
    public void NewGameData(int slot)
    {
        ClearBattle();
        playerLogic.Clear();
        ChapterRecordCollection collect = new ChapterRecordCollection();
        collect.Slot = slot;
        ChapterDef = ResourceManager.GetChapterDef(0);
        SaveChapterData(slot);
    }
    public void LoadChapterData(int slot)
    {
        ClearBattle();
        ChapterRecordCollection collect = record.LoadChapterFromDisk(slot);
        ChapterDef = ResourceManager.GetChapterDef(collect.CurrentTeam.Chapter);
        playerLogic.Clear();
        foreach (var v in collect.CurrentTeamPlayerInfo)
        {
            playerLogic.Add(new CharacterLogic(v));
        }
    }
    #endregion
}
