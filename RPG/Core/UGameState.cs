using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 保存当前GameMode下的一些数据,状态机在这个里面
/// </summary>
public class UGameState : UActor
{
    /// <summary>
    /// 战场上的我方玩家
    /// </summary>
    protected List<UPawn> LocalPlayers = new List<UPawn>();
    protected List<UPawn> LocalEnemies = new List<UPawn>();
    public int AddLocalPlayer(UPawn Player)
    {
        LocalPlayers.Add(Player);
        return LocalPlayers.Count;
    }
    public int AddLocalEnemy(UPawn Enemy)
    {
        LocalEnemies.Add(Enemy);
        return LocalEnemies.Count;
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
        if (LocalPlayers.Count <= Index)
        {
            Debug.LogError("The Index Player don't exists");
            return null;
        }
        if (LocalPlayers[Index] == null)
        {
            Debug.LogError("Null Pawn");
        }
        return LocalPlayers[Index];
    }
    public UPlayerController GetFirstLocalPlayerController()
    {
        return GetFirstGamePlayer().GetPlayerController<UPlayerController>();
    }
    List<UPawn> GetLocalPlayers()
    {
        return LocalPlayers;
    }
}
