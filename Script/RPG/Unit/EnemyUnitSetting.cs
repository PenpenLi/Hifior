using UnityEngine;
using System.Collections.Generic;

public class EnemyUnitSetting : MonoBehaviour
{
    public static readonly EnemyUnit EmptyUnit = new EnemyUnit(-1, -1);

    [System.Serializable]
    public struct EnemyUnit
    {
        public VInt2 Coord;
        public EnemyDef Enemy;
        public EnemyUnit(int x, int y, EnemyDef enemy = null)
        {
            Coord = new VInt2(x, y);
            Enemy = enemy;
        }
        public EnemyUnit(VInt2 p, EnemyDef enemy = null)
        {
            Coord = p;
            Enemy = enemy;
        }
    }
    /// <summary>
    /// 保存当前物体所包含的地方单位列表
    /// </summary>
    public List<EnemyUnit> Units;
    public bool Contains(VInt2 p)
    {
        foreach (EnemyUnit u in Units)
            if (u.Coord == p)
                return true;
        return false;
    }
    public int Remove(VInt2 p)
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
        return Contains(new VInt2(x, y));
    }
    public EnemyUnit GetUnit(VInt2 p)
    {
        foreach (EnemyUnit u in Units)
            if (u.Coord == p)
                return u;
        return EmptyUnit;
    }
    public EnemyUnit GetUnit(int x, int y)
    {
        return GetUnit(new VInt2(x, y));
    }
    public bool IsEmpty(EnemyUnit unit)
    {
        if (unit.Enemy == null || unit.Coord.x < 0 | unit.Coord.y < 0)
            return true;
        return false;
    }
    public EnemyDef GetDef(VInt2 p)
    {
        foreach (EnemyUnit u in Units)
            if (u.Coord == p)
                return u.Enemy;
        return null;
    }
    public EnemyDef GetDef(int x, int y)
    {
        return GetDef(new VInt2(x, y));
    }
    void OnDrawGizmosSelected()
    {
        foreach (EnemyUnit u in Units)
        {
            if (u.Enemy == null)
                GizmosUtil.GizmosDrawRect(5 + u.Coord.x * 10, 5 + u.Coord.y * 10, 10f, 10, 10, Color.red);
            else
                GizmosUtil.GizmosDrawRect(5 + u.Coord.x * 10, 5 + u.Coord.y * 10, 10f, 10, 10, Color.gray);
        }
    }
}