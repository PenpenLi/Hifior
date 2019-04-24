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
    public struct PlayerEntity
    {
        public List<RPGPlayer> Players;
        public List<CharacterLogic> PlayersLogic;
    }
    public struct EnemyEntity
    {
        public List<RPGEnemy> Enemies;
    }
    private PlayerEntity players;
    private EnemyEntity enemies;
    private Warehouse warehouse;
    public int MapId { get; private set; }
    public int TurnIndex { get; private set; }
    public EnumCharacterCamp TurnCamp { get; private set; }
    public ChapterManager()
    {
        players.Players = new List<RPGPlayer>();
        players.PlayersLogic = new List<CharacterLogic>();
        enemies.Enemies = new List<RPGEnemy>();
        warehouse = new Warehouse();
    }
    /// <summary>
    /// 载入地图事件
    /// </summary>
    /// <param name="mapId"></param>
    public void InitMapEvent(int mapId)
    {
        MapId = mapId;
        TurnIndex = 0;
    }
    /// <summary>
    /// 清除战场数据
    /// </summary>
    public void ClearBattle()
    {
        players.Players.Clear();
        enemies.Enemies.Clear();
    }
    public void NextTurn()
    {
        gameMode.LockInput(true);
        if (TurnIndex == 0)
        {
            TurnCamp = EnumCharacterCamp.Player;
            TurnIndex++;
            uiManager.TurnIndicate.Show(TurnCamp, TurnIndex, PlayerAction);
            return;
        }
        if (TurnCamp == EnumCharacterCamp.Player)
        {
            TurnCamp = EnumCharacterCamp.Enemy;
            uiManager.TurnIndicate.Show(TurnCamp, TurnIndex, EnemyAction);
        }
        else
        {
            TurnCamp = EnumCharacterCamp.Player;
            TurnIndex++;
            uiManager.TurnIndicate.Show(TurnCamp, TurnIndex, PlayerAction);
        }
    }
    /// <summary>
    /// 开始玩家行动
    /// </summary>
    public void PlayerAction()
    {
        gameMode.FreeBattleCamera();
        gameMode.LockInput(false);
    }
    /// <summary>
    /// 敌人AI行动
    /// </summary>
    public void EnemyAction()
    {

    }
    /// <summary>
    /// 仅添加数据
    /// </summary>
    public void AddPlayerLogic(PlayerDef def)
    {
        CharacterLogic logic = new CharacterLogic(def);
        players.PlayersLogic.Add(logic);
    }

    /// <summary>
    /// 添加到战场
    /// </summary>
    public void AddPlayerToBattle(RPGCharacter ch)
    {
        UnityEngine.Assertions.Assert.AreEqual(ch.GetCamp(), EnumCharacterCamp.Player);
        RPGPlayer player = ch as RPGPlayer;
        players.Players.Add(player);
        players.PlayersLogic.Add(player.Logic);
    }
    public void AddEnemyToBattle(RPGCharacter ch)
    {
        UnityEngine.Assertions.Assert.AreEqual(ch.GetCamp(), EnumCharacterCamp.Enemy);
        RPGEnemy player = ch as RPGEnemy;
        enemies.Enemies.Add(player);
    }
    public RPGCharacter GetCharacterFromCoord(Vector2Int tilePos)
    {
        foreach (var v in players.Players)
            if (v.GetTileCoord() == tilePos) return v;
        foreach (var v in enemies.Enemies)
            if (v.GetTileCoord() == tilePos) return v;
        return null;
    }
    public RPGCharacter GetCharacterFromCoord(Vector2Int tilePos, EnumCharacterCamp camp)
    {
        switch (camp)
        {
            case EnumCharacterCamp.Player:
                {
                    foreach (var v in players.Players)
                        if (v.GetTileCoord() == tilePos) return v;
                    break;
                }
            case EnumCharacterCamp.Enemy:
                {
                    foreach (var v in enemies.Enemies)
                        if (v.GetTileCoord() == tilePos) return v;
                    break;
                }
            case EnumCharacterCamp.Ally:
                {
                    foreach (var v in players.Players)
                        if (v.GetTileCoord() == tilePos) return v;
                    break;
                }
            case EnumCharacterCamp.NPC:
                {
                    foreach (var v in players.Players)
                        if (v.GetTileCoord() == tilePos) return v;
                    break;
                }
        }
        return null;
    }
    public RPGCharacter GetCharacterFromID(int id)
    {
        foreach (var v in players.Players)
            if (v.Logic.GetID() == id) return v;
        foreach (var v in enemies.Enemies)
            if (v.Logic.GetID() == id) return v;
        return null;
    }
    public bool HasCharacterFromID(int id)
    {
        return GetCharacterFromID(id) != null;
    }
    public void RemoveCharacter(RPGCharacter ch)
    {
        var camp = ch.GetCamp();
        if (camp == EnumCharacterCamp.Player)
        {
            players.Players.Remove(ch as RPGPlayer);
            ch.Logic.SetDead();
        }
        if (camp == EnumCharacterCamp.Enemy)
        {
            enemies.Enemies.Remove(ch as RPGEnemy);
        }
    }
    public RPGCharacter RemoveCharacterFromID(int id)
    {
        RPGCharacter rm = null;
        foreach (var v in players.Players)
            if (v.Logic.GetID() == id) rm= v;
        if (rm != null) { RemoveCharacter(rm); }
        foreach (var v in enemies.Enemies)
            if (v.Logic.GetID() == id) rm=v;
        if (rm != null) { RemoveCharacter(rm); }
        return null;
    }
    public bool HasCharacterFromCoord(Vector2Int tilePos)
    {
        return GetCharacterFromCoord(tilePos) != null;
    }
    public bool HasCharacterFromCoord(List<Vector2Int> tilePos)
    {
        foreach (var v in tilePos)
        {
            if (GetCharacterFromCoord(v) != null) return true;
        }
        return false;
    }
    public bool HasCharacterFromCoord(List<Vector2Int> tilePos, List<EnumCharacterCamp> camps)
    {
        foreach (var camp in camps)
        {
            foreach (var v in tilePos)
            {
                if (GetCharacterFromCoord(v, camp) != null)
                    return true;
            }
        }
        return false;
    }

    #region Money
   public void AddTeamMoney(int team ,int amount)
    {

    }
    public void AddCurrentTeamMoney( int amount)
    {

    }
    #endregion
    #region GameRecord

    private static GameRecord record = new GameRecord();
    private void CheckRecordError()
    {
        if (record == null) Debug.LogError("GameRecord is null");
        if (ChapterDef == null) Debug.LogError("ChapterDef is null");
    }
    public void NewGameData(int slot)
    {
        ClearBattle();
        ChapterRecordCollection collect = new ChapterRecordCollection();
        collect.Slot = slot;
        ChapterDef = ResourceManager.GetChapterDef(0);
        SaveChapterData(slot);
        LoadChapterData(slot);
    }
    /// <summary>
    /// 仅章节结束后的数据
    /// </summary>
    public void SaveChapterData(int slot)
    {
        CheckRecordError();
        List<CharacterInfo> playerInfos = new List<CharacterInfo>();
        foreach (var v in players.PlayersLogic)
        {
            v.Info.AfterBattle();
            playerInfos.Add(v.Info);
        }
        record.SaveChapter(slot, ChapterDef.TeamIndex, true, ChapterDef.CommonProperty.ID, warehouse, playerInfos);
    }
    public void LoadChapterData(int slot)
    {
        ClearBattle();
        players.PlayersLogic.Clear();
        ChapterRecordCollection collect = record.LoadChapterFromDisk(slot);
        ChapterDef = ResourceManager.GetChapterDef(collect.CurrentTeam.Chapter);
        ChapterDef.Event = GameObject.Instantiate(ChapterDef.Event);
        foreach (var v in collect.CurrentTeamPlayerInfo)
        {
            players.PlayersLogic.Add(new CharacterLogic(v));
        }
    }
    /// <summary>
    /// 记录战场情况
    /// </summary>
    public void SaveBattleData()
    {
        CheckRecordError();
        List<CharacterInfo> playerInfos = new List<CharacterInfo>();
        foreach (var v in players.PlayersLogic)
        {
            playerInfos.Add(v.Info);
        }

        record.SaveBattle(ChapterDef.TeamIndex, ChapterDef.CommonProperty.ID, MapId, warehouse, playerInfos, Event.EventInfo);
    }
    /// <summary>
    /// 载入事件数据，加载我方和敌方数据，并加载单位到战场上。
    /// </summary>
    /// <param name="slot"></param>
    public void LoadBattleData(int slot)
    {
        ClearBattle();
        BattleInfoCollection data = record.LoadBattleFromDisk(slot);
        //Debug.Log(collect.Event.TurnEvent[0].SequenceName);
        ChapterDef = ResourceManager.GetChapterDef(data.CurrentTeam.Chapter);
        gameMode.LoadTileMap(data.MapID);
        players.PlayersLogic.Clear();
        foreach (var v in data.CurrentTeamPlayerInfo)
        {
            RPGPlayer p = RPGPlayer.Create(v);
            players.PlayersLogic.Add(p.Logic);
            gameMode.AddUnitToMap(p, v.tileCoords);
        }
    }
    #endregion


}
