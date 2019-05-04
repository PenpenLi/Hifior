using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 管理章节数据和检测
/// 人员加载配置，各种事件触发等
/// </summary>
public class ChapterManager : ManagerBase
{
    public UnityAction<EnumCharacterCamp, int, UnityAction> OnShowTurnIndicate;
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
    private PlayerEntity players; public int PlayerCount { get { return players.Players.Count; } }
    private EnemyEntity enemies; public int EnemyCount { get { return enemies.Enemies.Count; } }
    private Warehouse warehouse;
    public int ChapterId { get; private set; }
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
            TurnIndex++;
            TurnCamp = EnumCharacterCamp.Player;
            EnableAllPlayerAction();
            CheckTurnEvent(TurnIndex, TurnCamp);
            return;
        }
        if (TurnCamp == EnumCharacterCamp.Player)
        {
            TurnCamp = EnumCharacterCamp.Enemy;
            DisableAllPlayerAction();
            CheckTurnEvent(TurnIndex, TurnCamp);
        }
        else
        {
            TurnIndex++;
            TurnCamp = EnumCharacterCamp.Player;
            EnableAllPlayerAction();
            CheckTurnEvent(TurnIndex, TurnCamp);
        }
    }
    /// <summary>
    /// 开始玩家行动
    /// </summary>
    public void StartPlayerAction()
    {
        gameMode.FreeBattleCamera();
        gameMode.LockInput(false);
    }
    /// <summary>
    /// 敌人AI行动
    /// </summary>
    public void StartEnemyAction()
    {
        gameMode.LockInput(true);
        //以Sequence的形式播放敌方行动，一个一个来，中间可以被打断直接输出结果
        foreach(var v in chapterManager.GetAllCharacters(EnumCharacterCamp.Enemy))
        {
            //v.AI_Attack.Action();
        }
        //结束后解锁各种操作镜头等
    }

    public void CheckTurnEvent(int round, EnumCharacterCamp camp)
    {
        var turnEvent = Event.EventInfo.GetTurnEvent(round, camp);
        if (AppConst.DebugMode)
        {
            if (turnEvent == null) Debug.Log("没有Turn事件");
            else Debug.Log("找到相匹配的Turn Event" + turnEvent);
        }
        UnityAction onCompleteTurnEvent = null;
        if (camp == EnumCharacterCamp.Player) onCompleteTurnEvent = StartPlayerAction;
        if (camp == EnumCharacterCamp.Enemy) onCompleteTurnEvent = StartEnemyAction;
        if (turnEvent == null || turnEvent.Sequence == null)
        {
            OnShowTurnIndicate(camp, round, onCompleteTurnEvent);
            return;
        }

        //如果有事件发生，则在事件发生后显示回合条
        {
            gameMode.BeforePlaySequence();
            turnEvent.Execute(chapterManager.Event.EventInfo, () =>
            {
                gameMode.AfterPlaySequence();
                OnShowTurnIndicate(camp, round, onCompleteTurnEvent);
            });
        }
    }

    /// <summary>
    /// 检测某个角色死亡时触发的事件
    /// </summary>
    public void CheckEnemyDeadEvent(int deadId, UnityAction onComplete)
    {
        var enemyEvent = Event.EventInfo.GetEnemyDieEvent(deadId);
        if (AppConst.DebugMode)
        {
            if (enemyEvent == null) Debug.Log("没有Enemy Dead事件 ID = " + deadId);
            else Debug.Log("找到相匹配的EnemyDead Event" + deadId);
        }
        if (enemyEvent == null || enemyEvent.Sequence == null)
        {
            onComplete?.Invoke();
            return;
        }
        {
            gameMode.BeforePlaySequence();
            enemyEvent.Execute(chapterManager.Event.EventInfo, () =>
            {
                gameMode.AfterPlaySequence();
                onComplete?.Invoke();
            });
        }
    }
    //先检测胜利，然后检测是否是少于
    public void CheckEnemyLessEvent(int enemyCount, UnityAction onComplete)
    {
        var enemyEvent = Event.EventInfo.GetEnemiesLessEvent(enemyCount);
        if (AppConst.DebugMode)
        {
            if (enemyEvent == null) Debug.Log("没有EnemyLess事件");
            else Debug.Log("找到相匹配的EnemyLess Event" + enemyCount);
        }
        if (enemyEvent == null || enemyEvent.Sequence == null)
        {
            Debug.LogError("事件为空");
            onComplete?.Invoke();
            return;
        }
        {
            gameMode.BeforePlaySequence();
            enemyEvent.Execute(chapterManager.Event.EventInfo, () =>
            {
                gameMode.AfterPlaySequence();
                onComplete?.Invoke();
            });
        }
    }

    #region 章节设置 检查胜利

    public bool CheckWin_KillAllEnemy()
    {
        if (HasWinCondition(EnumWinCondition.全灭敌人))// && GetGameStatus<GS_Battle>().GetNumLocalEnemies() == 0
            return true;
        else
            return false;
    }
    /// <summary>
    /// 每一个加入的Boss都有一个唯一的ID，从0开始增长
    /// </summary>
    /// <param name="BossID"></param>
    /// <returns></returns>
    public bool CheckWin_DefeatBoss(int BossID)
    {
        if (HasWinCondition(EnumWinCondition.击败指定Boss))
            return true;
        return false;
    }
    /// <summary>
    /// 在Sequence里添加Start() 在Start里将GameMode 里的当前CityID加入
    /// </summary>
    /// <returns></returns>
    public bool CheckWin_Seize(int CityID = 0)
    {
        if (HasWinCondition(EnumWinCondition.压制指定城池) && ChapterDef.WinCondition.CityID == CityID)
            return true;
        return false;
    }
    public bool CheckWin_Leave()
    {
        if (HasWinCondition(EnumWinCondition.领主地点撤离))
            return true;
        return false;
    }
    public bool CheckWin_Round(int Round)
    {
        if (HasWinCondition(EnumWinCondition.回合坚持) && Round == ChapterDef.WinCondition.Round)
            return true;
        return false;
    }
    public List<string> GetWinConditionText()
    {
        List<string> texts = new List<string>();
        List<EnumWinCondition> L = GetAllWinCondition();
        if (L.Contains(EnumWinCondition.全灭敌人))
            texts.Add("击败所有敌方单位");
        if (L.Contains(EnumWinCondition.击败全部Boss))
            texts.Add("击败所有Boss");
        if (L.Contains(EnumWinCondition.击败指定Boss))
            texts.Add("击败" + ResourceManager.GetEnemyDef(ChapterDef.WinCondition.BossID));
        if (L.Contains(EnumWinCondition.回合坚持))
            texts.Add("坚持" + ChapterDef.WinCondition.Round + "个回合");
        if (L.Contains(EnumWinCondition.领主地点撤离))
            texts.Add("在指定地点撤离");
        if (L.Contains(EnumWinCondition.压制指定城池))
            texts.Add("压制指定城池");
        if (L.Contains(EnumWinCondition.压制所有城池))
            texts.Add("压制所有城池");
        return texts;
    }
    public List<EnumWinCondition> GetAllWinCondition()
    {
        int max = 6;
        List<EnumWinCondition> L = new List<EnumWinCondition>();
        for (int i = 0; i < max; i++)
        {
            if (EnumTables.MaskFieldIdentify(ChapterDef.WinCondition.Condition, i))
            {
                L.Add((EnumWinCondition)i);
            }
        }
        return L;
    }
    public bool HasWinCondition(EnumWinCondition Condition)
    {
        return EnumTables.MaskFieldIdentify(ChapterDef.WinCondition.Condition, (int)Condition);
    }
    #endregion

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
    public List<RPGCharacter> GetAllCharacters()
    {
        List<RPGCharacter> r = new List<RPGCharacter>();
        foreach (var v in players.Players)
            r.Add(v);
        foreach (var v in enemies.Enemies)
            r.Add(v);
        return r;
    }
    public List<RPGCharacter> GetAllCharacters(EnumCharacterCamp camp)
    {
        List<RPGCharacter> r = new List<RPGCharacter>();
        switch (camp)
        {
            case EnumCharacterCamp.Player:
                {
                    foreach (var v in players.Players)
                        r.Add(v);
                    return r;
                }
            case EnumCharacterCamp.Enemy:
                {
                    foreach (var v in enemies.Enemies)
                        r.Add(v);
                    return r;
                }
            case EnumCharacterCamp.Ally:
                break;
            case EnumCharacterCamp.NPC:
                break;
            default:
                break;
        }
        return r;
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
    public List<RPGCharacter> GetSidewayCharacter(Vector2Int tilePos)
    {
        List<RPGCharacter> r = new List<RPGCharacter>();
        var sideways = PositionMath.GetSidewayTilePos(tilePos);
        foreach (var v in sideways)
        {
            var ch = GetCharacterFromCoord(v);
            if (ch != null)
                r.Add(ch);
        }
        return r;
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
            if (v.Logic.GetID() == id) rm = v;
        if (rm != null) { RemoveCharacter(rm); }
        foreach (var v in enemies.Enemies)
            if (v.Logic.GetID() == id) rm = v;
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
    public void EnableAllPlayerAction()
    {
        foreach (var v in players.Players)
        {
            v.EnableAction(true);
        }
    }
    public void DisableAllPlayerAction()
    {
        foreach (var v in players.Players)
        {
            v.DisableAction(false);
            v.NormalSprite();//虽然是不让行动，但是显示要恢复正常的显示
        }
    }
    #region Money
    public void AddTeamMoney(int team, int amount)
    {

    }
    public void AddCurrentTeamMoney(int amount)
    {

    }
    #endregion
    #region GameRecord

    private static GameRecord record = new GameRecord();
    public GameRecord Record { get { return record; } }
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
        LoadChapterDef(collect.CurrentTeam.Chapter);
        foreach (var v in collect.CurrentTeamPlayerInfo)
        {
            players.PlayersLogic.Add(new CharacterLogic(v));
        }
    }
    public void LoadChapterDef(int id)
    {
        if (ChapterDef != null && Event.gameObject != null) GameObject.Destroy(Event.gameObject);
        ChapterDef = ResourceManager.GetChapterDef(id);
        ChapterDef.Event = GameObject.Instantiate(ChapterDef.Event);
        ChapterId = id;
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
            gameMode.BattlePlayer.AddUnitToMap(p, v.tileCoords);
        }
    }
    #endregion


}
