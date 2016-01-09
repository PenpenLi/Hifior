using UnityEngine;
using System.Collections.Generic;

public class ChapterSettingDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;
    public Sprite Icon;
    public int MaxPlayerCount;
    public List<int> ForceInvolve;
    public EnumWeather Weather;
    public bool Preparation;
    public WinCondition WinCondition;
    public FailCondition FailCondition;
    /// <summary>
    /// 包含 我方行动，敌方行动，同盟军行动，我军战斗，敌军战斗，同盟军战斗，出击准备7个
    /// </summary>
    public List<int> BGMSetting;
    /// <summary>
    /// 准备画面的武器店武器列表
    /// </summary>
    public List<int> WeaponRoom;
    /// <summary>
    /// 准备画面的道具店道具列表
    /// </summary>
    public List<int> PropRoom;
    public ChapterSettingDef()
    {
        ForceInvolve = new List<int>();
        BGMSetting = new List<int>();
        WeaponRoom = new List<int>();
        PropRoom = new List<int>();
    }
}
[System.Serializable]
public class WinCondition
{
    public int Condition;
    public int BossID;
    public int CityID;
    public int Round;
    public int WinX, WinY;
    public WinCondition(int condition, int bossID, int cityID, int round, int winX, int winY)
    {
        Condition = condition;
        BossID = bossID;
        CityID = cityID;
        Round = round;
        WinX = winX;
        WinY = winY;
    }
}
[System.Serializable]
public class FailCondition
{
    public int Condition;
    public int PlayerID;
    public int Round;
    public int CityID;
    public FailCondition(int condition, int playerID, int round, int cityID)
    {
        Condition = condition;
        PlayerID = playerID;
        Round = round;
        CityID = cityID;
    }
}
