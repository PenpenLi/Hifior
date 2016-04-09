using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 游戏状态，包含战场上有多少我方和敌方角色，地图事件状态，多少回合。
/// </summary>
public class GS_Battle : UGameStatus
{
    private List<RPGCharacter> LocalPlayerAllys = new List<RPGCharacter>();
    private T GetCharacterByID<T>(List<RPGCharacter> CharacterList, int ID) where T : RPGCharacter
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            T Character = CharacterList[i] as T;
            if (Character.GetCharacterID().Equals(ID))
            {
                return Character;
            }
        }
        return null;
    }
    private T GetCharacterByPosition<T>(List<RPGCharacter> CharacterList, Point2D TilePosition) where T : RPGCharacter
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            T Character = CharacterList[i] as T;
            if (Character.GetTileCoord().Equals(TilePosition))
            {
                return Character;
            }
        }
        return null;
    }
    public RPGPlayer GetPlayer(int PlayerID)
    {
        return GetCharacterByID<RPGPlayer>(LocalPlayers, PlayerID);
    }

    /// <summary>
    /// 在坐标处找到玩家,找不到返回NULL
    /// </summary>
    /// <param name="TilePosition"></param>
    /// <returns></returns>
    public RPGPlayer GetPlayerAt(Point2D TilePosition)
    {
        return GetCharacterByPosition<RPGPlayer>(LocalPlayers, TilePosition);
    }
    public RPGEnemy GetEnemy(int EnemyID)
    {
        return GetCharacterByID<RPGEnemy>(LocalEnemies, EnemyID);
    }
    /// <summary>
    /// 在坐标处找到敌方,找不到返回NULL
    /// </summary>
    /// <param name="TilePosition"></param>
    /// <returns></returns>
    public RPGEnemy GetEnemyAt(Point2D TilePosition)
    {
        return GetCharacterByPosition<RPGEnemy>(LocalEnemies, TilePosition);
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
    /// <summary>
    /// 使我方的角色可以被控制
    /// </summary>
    public void EnableAllPlayerControl()
    {
        foreach (RPGCharacter Character in LocalPlayers)
        {
            Character.EnableControl();
        }
    }
    /// <summary>
    /// 获取相邻单位的角色
    /// </summary>
    public List<RPGCharacter> GetNeighbors(Point2D TilePosition)
    {
        List<RPGCharacter> Temp = new List<RPGCharacter>();
        for (int i = 0; i < LocalPlayers.Count; i++)
        {
            if (Point2D.GetDistance(LocalPlayers[i].GetTileCoord(), TilePosition) == 1)
            {
                Temp.Add(LocalPlayers[i]);
            }
        }
        for (int i = 0; i < LocalEnemies.Count; i++)
        {
            if (Point2D.GetDistance(LocalEnemies[i].GetTileCoord(), TilePosition) == 1)
            {
                Temp.Add(LocalEnemies[i]);
            }
        }
        return Temp;
    }
}
