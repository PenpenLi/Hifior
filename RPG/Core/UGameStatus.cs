using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 保存当前GameMode下的一些数据,状态机在这个里面
/// </summary>
public class UGameStatus : UActor
{
    /// <summary>
    /// 战场上的我方玩家
    /// </summary>
    protected List<RPGCharacter> LocalPlayers = new List<RPGCharacter>();
    protected List<RPGCharacter> LocalEnemies = new List<RPGCharacter>();

    public int AddLocalPlayer(RPGCharacter Player)
    {
        LocalPlayers.Add(Player);
        return LocalPlayers.Count;
    }
    public int AddLocalEnemy(RPGCharacter Enemy)
    {
        LocalEnemies.Add(Enemy);
        return LocalEnemies.Count;
    }
    /// <summary>
    /// 根据ID递增排序
    /// </summary>
    /// <param name="PawnList"></param>
    public void SortCharacterByID(List<RPGCharacter> PawnList)
    {
        LocalPlayers.Sort((RPGCharacter x, RPGCharacter y) =>
        {
            if (x.GetCharacterID() > y.GetCharacterID())
            {
                return 1;
            }
            else if (x.GetCharacterID() == y.GetCharacterID())
            {
                return 0;
            }
            else
            {
                return -1;
            }
        });
    }
    public int GetNumLocalPlayers()
    {
        return LocalPlayers.Count;
    }
    public int GetNumLocalEnemies()
    {
        return LocalEnemies.Count;
    }
    public UPawn GetFirstGamePlayer()
    {
        if (LocalPlayers.Count > 0)
            return LocalPlayers[0];
        Debug.LogError("No Player exists");
        return null;
    }
    public UPawn GetLocalPlayerByIndex(int Index)
    {
      return  GetLocalPawnByIndex<RPGCharacter>(Index, LocalPlayers);
    }
    public UPawn GetLocalEnemyByIndex(int Index)
    {
        return GetLocalPawnByIndex<RPGCharacter>(Index, LocalEnemies);
    }
    protected T GetLocalPawnByIndex<T>(int Index,List<RPGCharacter> LocalPawns)where T :RPGCharacter
    {
        if (LocalPawns.Count <= Index)
        {
            Debug.LogError("The Index Player don't exists");
            return null;
        }
        if (LocalPawns[Index] == null)
        {
            Debug.LogError("Null Pawn");
        }
        return LocalPawns[Index] as T;
    }
    public UPlayerController GetFirstLocalPlayerController()
    {
        return GetFirstGamePlayer().GetPlayerController<UPlayerController>();
    }
    List<RPGCharacter> GetLocalPlayers()
    {
        return LocalPlayers;
    }
}
