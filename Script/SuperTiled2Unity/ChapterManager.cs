using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 管理章节数据和检测
/// 人员加载配置，各种事件触发等
/// </summary>
public class ChapterManager : ManagerBase
{
    private SLGChapter chapterSettting;
    public GameRecord recorder;
    private List<RPGPlayer> players;
    private List<RPGEnemy> enemies;
    private List<CharacterLogic> playerLogic;
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
    }
    /// <summary>
    /// 清除战场数据
    /// </summary>
    public void ClearBattle(bool save)
    {
        players.Clear();
        enemies.Clear();
        if (save)
        {
            SaveBattleToLogic();
        }
    }
    public void SaveBattleToLogic()
    {
        foreach (var v in playerLogic)
        {
            playerLogic.Add(players[0].Logic());
        }
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
        playerLogic.Add(player.Logic());
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
}
