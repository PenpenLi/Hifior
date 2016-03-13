using UnityEngine;
using System.Collections;

public class RPGCharacter : RPGCharacterBase
{
    public ItemGroup Item;

    protected Animator anim;
    [SerializeField]
    protected bool bRunning = false;//是否在移动中

    protected int damageCount = 0;//收到伤害和造成伤害的次数
    [SerializeField]
    protected Point2D tileCoords;
    public RPGCharacter()
    {
        Item = new ItemGroup(base.Attribute);
    }
    public Point2D GetTileCoord()
    {
        return tileCoords;
    }
    public void SetTileCoord(int x,int y)
    {
        tileCoords.x = x;
        tileCoords.y = y;
    }
    public bool IsRunning()
    {
        return bRunning;
    }
    public virtual void Run()
    {
        bRunning = true;
    }
    public virtual void StopRun()
    {
        bRunning = false;
    }
    public int GetCareer()
    {
        return 0;
    }
}
