﻿using UnityEngine;
using System.Collections.Generic;

public class EnemyUnitSetting : MonoBehaviour
{
    public readonly EnemyUnit EmptyUnit= new  EnemyUnit(-1,-1);

    [System.Serializable]
    public struct EnemyUnit
    {
        public Point2D Coord;
        public EnemyDef Enemy;
        public EnemyUnit(int x, int y, EnemyDef enemy = null)
        {
            Coord = new Point2D(x, y);
            Enemy = enemy;
        }
        public EnemyUnit(Point2D p, EnemyDef enemy = null)
        {
            Coord = p;
            Enemy = enemy;
        }
    }
    /// <summary>
    /// 保存当前物体所包含的地方单位列表
    /// </summary>
    public List<EnemyUnit> Units;
    public bool Contains(Point2D p)
    {
        foreach (EnemyUnit u in Units)
            if (u.Coord == p)
                return true;
        return false;
    }
    public int Remove(Point2D p)
    {
        int at = -1;
        for (int i = 0; i < Units.Count; i++)
        {
            if (Units[i].Coord == p)
            {
                at = i;
            }
        }
        if (at > -1)
            Units.RemoveAt(at);
        return at;
    }
    public bool Contains(int x, int y)
    {
        return Contains(new Point2D(x, y));
    }
    public EnemyDef GetDef(Point2D p)
    {
        foreach (EnemyUnit u in Units)
            if (u.Coord == p)
                return u.Enemy;
        return null;
    }
    public EnemyUnit GetUnit(Point2D p)
    {
        foreach (EnemyUnit u in Units)
            if (u.Coord == p)
                return u;
        return EmptyUnit;
    }
    public EnemyUnit GetUnit(int x,int y)
    {
        return GetUnit(new Point2D(x, y));
    }
    public bool IsEmpty(EnemyUnit unit)
    {
        if (unit.Enemy == null|| unit.Coord.x<0 |unit.Coord.y<0)
            return true;
        return false;
    }
    public EnemyDef GetDef(int x, int y)
    {
        return GetDef(new Point2D(x, y));
    }
    void OnDrawGizmosSelected()
    {
        foreach (EnemyUnit u in Units)
        {
            GizmosUtil.GizmosDrawRect(5 + u.Coord.x * 10, 5 + u.Coord.y * 10, 10f, 10, 10, Color.cyan);
        }
    }
}