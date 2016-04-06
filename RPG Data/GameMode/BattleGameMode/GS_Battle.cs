using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏状态，包含战场上有多少我方和敌方角色，地图事件状态，多少回合。
/// </summary>
public class GS_Battle : UGameState
{
    private List<UPawn> LocalPlayerAllys = new List<UPawn>();

    /// <summary>
    /// 在坐标处找到玩家,找不到返回NULL
    /// </summary>
    /// <param name="TilePosition"></param>
    /// <returns></returns>
    public RPGPlayer GetPlayerAt(Point2D TilePosition)
    {
        for (int i = 0; i < LocalPlayers.Count; i++)
        {
            RPGPlayer Character = LocalPlayers[i] as RPGPlayer;
            if (Character.GetTileCoord().Equals(TilePosition))
            {
                return Character;
            }
        }
        return null;
    }
    /// <summary>
    /// 在坐标处找到敌方,找不到返回NULL
    /// </summary>
    /// <param name="TilePosition"></param>
    /// <returns></returns>
    public RPGEnemy GetEnemyAt(Point2D TilePosition)
    {
        for (int i = 0; i < LocalEnemies.Count; i++)
        {
            RPGEnemy Character = LocalEnemies[i] as RPGEnemy;
            if (Character.GetTileCoord().Equals(TilePosition))
            {
                return Character;
            }
        }
        return null;
    }
    /// <summary>
    /// 在坐标处找到我方或者敌方或者NPC,找不到返回NULL
    /// </summary>
    /// <param name="TilePosition"></param>
    /// <returns></returns>
    public RPGCharacter GetAnyUnitAt(Point2D TilePosition)
    {
        RPGCharacter Character = GetPlayerAt(TilePosition);
        if (Character == null)
        {
            Character = GetEnemyAt(TilePosition);
        }
        return Character;
    }
}
