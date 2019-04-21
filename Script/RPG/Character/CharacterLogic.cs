using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterBattleInfo
{
    public enum EBattleActionType
    {
        Attack,
        Skill,
        Heal,
        Stole
    }
    /// <summary>
    /// 是否已经选择了目标Tile
    /// </summary>
    bool hasChooseTargetPos;
    /// <summary>
    /// 有没有可以生效的单位在范围内
    /// </summary>
    bool hasTargetInRange;
    /// <summary>
    /// 生效的对象
    /// </summary>
    int effectSide;
    /// <summary>
    /// 本身的位置
    /// </summary>
    Vector2Int selfTilePos;
    /// <summary>
    /// 选择的目标Tile位置
    /// </summary>
    Vector2Int selectTilePos;
    /// <summary>
    /// 目标位置处可以被选择的范围
    /// </summary>
    List<Vector2Int> targetChooseRanges; public List<Vector2Int> TargetChooseRanges { get { return targetChooseRanges; } }
    /// <summary>
    /// 目标位置处可以作用的范围
    /// </summary>
    List<Vector2Int> targetEffectRanges; public List<Vector2Int> TargetEffectRanges { get { return targetEffectRanges; } }
    EBattleActionType battleActionType; public EBattleActionType BattleActionType { get { return battleActionType; } }
    EnumSelectEffectRangeType selectRangeType;
    EnumSelectEffectRangeType effectRangeType;
    Vector2Int selectRange;
    Vector2Int effectRange;
    public void SetSelectTargetParam(EBattleActionType actionType, Vector2Int selfPos, EnumSelectEffectRangeType selRangeType, Vector2Int selRange, EnumSelectEffectRangeType effRangeType, Vector2Int effRange)
    {
        battleActionType = actionType;
        selfTilePos = selfPos;
        selectRangeType = selRangeType;
        effectRangeType = effRangeType;
        selectRange = selRange;
        effectRange = effRange;
        targetChooseRanges = GetTargetSelectRange(selfTilePos, selRangeType, selectRange);
    }
    public void SetEffectTarget(Vector2Int tilePos)
    {
        selectTilePos = tilePos;
        targetEffectRanges = GetTargetSelectRange(selectTilePos, effectRangeType, effectRange);
    }
    public List<EnumCharacterCamp> GetEffectCamps()
    {
        List<EnumCharacterCamp> r = new List<EnumCharacterCamp>();
        switch (battleActionType)
        {
            case EBattleActionType.Attack:
                r.Add(EnumCharacterCamp.Enemy);
                break;
            case EBattleActionType.Heal:
                r.Add(EnumCharacterCamp.Player);
                break;
            case EBattleActionType.Stole:
                r.Add(EnumCharacterCamp.Enemy);
                break;
        }
        return r;
    }
    public static List<Vector2Int> GetTargetSelectRange(Vector2Int pos, EnumSelectEffectRangeType rangeType, Vector2Int range)
    {
        List<Vector2Int> attackArea = new List<Vector2Int>();
        int x = pos.x;
        int y = pos.y;
        int TileWidth = PositionMath.TileWidth;
        int TileHeight = PositionMath.TileHeight;
        attackArea.Clear();

        var RangeMax = range.y;
        var RangeMin = range.x;
        UnityEngine.Assertions.Assert.IsTrue(RangeMax >= RangeMin);

        if (RangeMin > 1)
        {
            int left = (x - RangeMax) < 0 ? 0 : x - RangeMax;
            int right = (x + RangeMax) > TileWidth - 1 ? TileWidth - 1 : x + RangeMax;
            int up = (y - RangeMax < 0) ? 0 : y - RangeMax;
            int bottom = (y + RangeMax) > TileHeight - 1 ? TileHeight - 1 : y + RangeMax;
            if (rangeType == 0)
            {
                for (int i = left; i <= right; i++)
                {
                    for (int j = up; j <= bottom; j++)
                    {
                        int absLen = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                        if (absLen < RangeMin || absLen > RangeMax || attackArea.Contains(new Vector2Int(i, j)))
                            continue;
                        if (i >= 0 && j >= 0)
                            attackArea.Add(new Vector2Int(i, j));
                    }
                }
            }
            if (rangeType == EnumSelectEffectRangeType.十字形)//为1则是只能上下左右寻找目标
            {
                for (int i = left; i <= right; i++)//得到x轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - x);
                    if (absLen < RangeMin || absLen > RangeMax || attackArea.Contains(new Vector2Int(i, y)))
                        continue;
                    if (i >= 0 && y >= 0)
                        attackArea.Add(new Vector2Int(i, y));

                }
                for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - y);
                    if (absLen < RangeMin || absLen > RangeMax || attackArea.Contains(new Vector2Int(x, i)))
                        continue;
                    if (x >= 0 && i >= 0)
                        attackArea.Add(new Vector2Int(x, i));
                }
            }
            if (rangeType == EnumSelectEffectRangeType.正方形)//为2矩形攻击范围
            {
                for (int i = left; i <= right; i++)
                {
                    for (int j = up; j <= bottom; j++)
                    {
                        int absX = Mathf.Abs(i - x);
                        int absY = Mathf.Abs(j - y);
                        if (absX < RangeMin && absY < RangeMin)//在其中xy均小于最小坐标的不符合，直接进行下一个循环
                            continue;
                        if (i >= 0 && j >= 0)
                            attackArea.Add(new Vector2Int(i, j));
                    }
                }
            }
            /* if (rangeType == 3)//全屏攻击。放到外面单独处理，暂时用不到
             {
                 for (int i = 0; i < mapTileX; i++)
                 {
                     for (int j = 0; j < mapTileX; j++)
                     {
                         AttackAreaPoints.Add(new Point2D(i, j));
                     }
                 }
             }*/
        }
        else //如果是最小攻击距离从1开始
        {

            for (int i = -RangeMax; i <= RangeMax; i++)
            {
                for (int j = -RangeMax; j <= RangeMax; j++)
                {
                    switch (rangeType)
                    {
                        case EnumSelectEffectRangeType.菱形:
                            if (Mathf.Abs(i) + Mathf.Abs(j) > RangeMax) { continue; }
                            break;
                        case EnumSelectEffectRangeType.十字形:
                            if (Mathf.Abs(i) != 0 && Mathf.Abs(j) != 0) { continue; }
                            break;
                        case EnumSelectEffectRangeType.正方形:
                            break;
                    }
                    if (i + x >= 0 && j + y >= 0)
                        attackArea.Add(new Vector2Int(i + x, j + y));
                }
            }
            if (RangeMin == 1)
                attackArea.Remove(new Vector2Int(x, y));
        }
        return attackArea;
    }
}
public class CharacterLogic
{
    public CharacterLogic(PlayerDef def)
    {
        characterDef = def;
        careerDef = ResourceManager.GetCareerDef(characterDef.Career);
        Info = new CharacterInfo(def);
        BattleInfo = new CharacterBattleInfo();
    }
    public CharacterLogic(CharacterInfo info)
    {
        Info = info;
        var id = info.ID;
        careerDef = ResourceManager.GetCareerDef(info.Career);
        characterDef = ResourceManager.GetPlayerDef(id);
    }
    /// <summary>
    /// 包含需要被序列化记录的数据
    /// </summary>
    public CharacterInfo Info { private set; get; }
    public PlayerDef characterDef;
    public CareerDef careerDef;
    public CharacterBattleInfo BattleInfo { private set; get; }
    public bool hasFinishAction { private set; get; }
    /// <summary>
    /// 是否可以操控行动，行动完毕或者被石化，冻住等则为False
    /// </summary>
    protected bool bEnableAction = true;

    /// <summary>
    /// 是否可以被玩家选择并进行行动
    /// </summary>
    public bool Controllable { get { return Info.Camp == EnumCharacterCamp.Player && !hasFinishAction && bEnableAction; } }
    /// <summary>
    /// 是否在移动中
    /// </summary>
    protected bool bRunning = false;
    /// <summary>
    /// 是否在攻击过程中
    /// </summary>
    protected bool bAttacking = false;
    protected int damageCount = 0;//收到伤害和造成伤害的次数

    #region get
    public EnumMoveClassType GetMoveClass()
    {
        return careerDef.MoveClass;
    }
    public int GetMovement()
    {
        return 6;
    }
    public Vector2Int GetTileCoord()
    {
        return Info.tileCoords;
    }
    public Vector2Int GetOldTileCoord()
    {
        return Info.oldTileCoords;
    }
    public EnumCharacterImportance Importance { get { return characterDef.CharacterImportance; } }
    public string GetName()
    {
        return characterDef.CommonProperty.Name;
    }
    public int GetDefaultCareer()
    {
        return characterDef.Career;
    }
    public CharacterAttribute GetDefaultAttribute()
    {
        return characterDef.DefaultAttribute;
    }
    public CharacterAttribute GetAttribute()
    {
        return Info.Attribute;
    }
    public int GetID()
    {
        return characterDef.CommonProperty.ID;
    }
    public string GetDescription()
    {
        return characterDef.CommonProperty.Description;
    }
    public Sprite GetPortrait()
    {
        return characterDef.Portrait;
    }
    public GameObject GetStaticMesh()
    {
        return characterDef.BattleModel;
    }
    public Sprite[] GetStaySprites()
    {
        return careerDef.Stay;
    }
    public Sprite[] GetMoveSprites()
    {
        return careerDef.Move;
    }
    public int GetLevel()
    {
        return Info.Level;
    }
    public int GetCareer()
    {
        return Info.Career;
    }
    public string GetCareerName()
    {
        return null;
    }
    public int GetMaxHP()
    {
        return Info.MaxHP;
    }
    public int GetCurrentHP()
    {
        return Info.Exp;
    }
    public int GetExp()
    {
        return Info.Exp;
    }

    public int GetPhysicalPower()
    {
        return Info.Attribute.PhysicalPower;
    }

    public int GetSkill()
    {
        return Info.Attribute.Skill;
    }

    public int GetSpeed()
    {
        return Info.Attribute.Speed;
    }

    public int GetLuck()
    {
        return Info.Attribute.Luck;
    }

    public int GetMagicalDefense()
    {
        return Info.Attribute.MagicalDefense;
    }

    public int GetPhysicalDefense()
    {
        return Info.Attribute.PhysicalDefense;
    }

    public int GetMagicalPower()
    {
        return Info.Attribute.MagicalPower;
    }
    #endregion
    #region set
    public void SetAttribute(CharacterAttribute InAttribute)
    {
        Info.Attribute = (CharacterAttribute)InAttribute.Clone();
        Info.Attribute.HP = InAttribute.HP;
    }
    public void SetTileCoord(Vector2Int tilePos)
    {
        Info.oldTileCoords = Info.tileCoords;
        Info.tileCoords = tilePos;
    }
    public void SetCareer(int career)
    {
        Info.Career = career;
    }
    public void SetLevel(int level)
    {
        Info.Level = level;
    }
    public void EndAction()
    {
        hasFinishAction = true;
    }
    public void StartAction()
    {
        hasFinishAction = false;
    }

    #endregion


    #region 人物战斗获取函数

    public int GetAttack()//攻击力等于自身的伤害加武器伤害
    {
        WeaponItem equipItem = Info.Items.GetEquipWeapon();
        if (equipItem == null)
            return 0;
        WeaponDef itemDef = ResourceManager.GetWeaponDef(equipItem.ID);
        var att = GetAttribute();
        int itemType = (int)itemDef.WeaponType;
        int power = itemDef.Power;
        if (itemType > 0 && itemType <= 4)
            return att.PhysicalPower + power;
        if (itemType > 4 && itemType <= 8)
            return att.MagicalPower + power;
        if (itemType > 8)
            return att.PhysicalPower + att.MagicalPower + power;
        return 0;
    }

    public int GetHit()
    {
        WeaponItem equipItem = Info.Items.GetEquipWeapon();
        var att = GetAttribute();
        return ResourceManager.GetWeaponDef(equipItem.ID).Hit + att.Skill;//武器命中+技术
    }
    public int GetCritical()
    {
        WeaponItem equipItem = Info.Items.GetEquipWeapon();
        var att = GetAttribute();
        return ResourceManager.GetWeaponDef(equipItem.ID).Crit + (att.Skill + att.Luck / 2) / 2;
    }
    public int GetAvoid()
    { //自身速度+自身幸运+支援效果+地形效果
        var att = GetAttribute();
        return att.Speed + att.Luck;//getMapAvoid()
    }
    public int GetCriticalAvoid()
    {
        var att = GetAttribute();
        return att.Luck;
    }
    public int GetRangeMax()
    {
        return ResourceManager.GetWeaponDef(Info.Items.GetEquipWeapon().ID).RangeType.SelectRange.y;
    }
    public int GetRangeMin()
    {
        return ResourceManager.GetWeaponDef(Info.Items.GetEquipWeapon().ID).RangeType.SelectRange.x;
    }
    public EnumSelectEffectRangeType GetSelectRangeType()
    {
        return ResourceManager.GetWeaponDef(Info.Items.GetEquipWeapon().ID).RangeType.SelectType;
    }
    public EnumSelectEffectRangeType GetEffectRangeType()
    {
        return ResourceManager.GetWeaponDef(Info.Items.GetEquipWeapon().ID).RangeType.EffectType;
    }
    public int GetAnger()
    {
        return 0;
    }
    public int GetAttackSpeed()
    {
        var att = GetAttribute();
        return att.Speed - ResourceManager.GetWeaponDef(Info.Items.GetEquipWeapon().ID).Weight;
    }

    public void SetDead()
    {
        Info.Alive = false;
    }
    public void SetAlive()
    {
        Info.Alive = true;
    }
    public bool IsAlive()
    {
        return Info.Alive;
    }
    #endregion

}
