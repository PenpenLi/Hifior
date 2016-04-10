using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 敌方自动行动控制器
/// </summary>
public class AC_EnemyRoundAction : UAIController
{
    public override void Reset()
    {
        base.Reset();

        CurrentIndex = 0;
        PossessAI(CurrentIndex);
    }
    public override void PostInitializeComponents()
    {
        base.PostInitializeComponents();
        Disable();
    }

    private enum AIACTION
    {
        AttackInRange = 0,// -         移动到可移动和攻击的范围内进行攻击 
        DontMoveAttackInAttackRange,// -         不移动，总是呆在原地，除非有人进入原地的攻击范围
        Stay,//只是待机不主动攻击
        AfterAttackedAttack,// -         不进行移动，受到攻击后改为主动寻找范围内可攻击的人物
        MoveAttackNearest,// -         主动朝最近的角色进行移动
        MoveAttackLeader,// -         主动朝主角位置进行移动
        MoveAttackWeaker,// -         主动寻找地图上的虚弱单位
        MoveLessAccess,//-         移动到我方只有一个人可以攻击的范围内
        MoveVillageTreasure,// -         主动破坏村庄或者偷取宝箱无视人物攻击，不进行主动攻击
        Heal,// -         找寻范围内HP不足的单位进行治疗，或者攻击敌人（如果没有需要回复的单位且身上有攻击武器，则按照0进行移动）
        DontMoveHeal,// -         不进行移动，依靠远程回复杖原地回复或攻击（如果没有需要回复的单位且身上的远程武器可以打到人）
    }
    private enum ATTACKPRIORITY
    {
        MaxDamage = 0,//-         找寻伤害最高的人物攻击。
        PossibilityToDie,//-         找寻攻击有可能致死的攻击
        MinHP,//-         找寻HP最少的人物攻击
        MaxHitRate,//-         找寻命中率最高的人物攻击
        Leader,//-         找寻主角攻击
        Nearest,//-         找寻最近的人物攻击
        Random,//-         随机寻找可攻击范围内的人物攻击
    }
    private enum HEALPROMISE
    {
        DontHeal = 0,//-         不进行自我治疗
        HalfHP,//-         如果身上有伤药，低于HP一半使用，否则向我方有治疗杖的单位移动，如果都没有，则战死
        OneThirdHP,//-         如果身上有伤药，低于HP三分之一使用，否则向我方有治疗杖的单位移动，如果都没有，则战死
        LesseHP,//-         如果身上有伤药，HP少了伤药可以治疗的部分则进行治疗，如果是特效药，直接等HP低于一半才使用
    }
    public Pawn_BattleArrow BattleArrow;

    protected int[] ai = new int[3] { 0, 0, 0 };
    AIACTION currentAI;
    public string m_strName;   //描述

    private int CurrentIndex;
    public List<RPGCharacter> InAtkRangeEnemyList;
    RPGCharacter NearestEnemy;
    RPGCharacter Leader;
    private Point2D AIPawnTilePos
    {
        get { return CurrentControledPawn.GetTileCoord(); }
    }
    private RPGCharacter CurrentControledPawn
    {
        get { return GetPawn() as RPGCharacter; }
    }
    private SLGMap _Map
    {
        get { return GetGameMode<GM_Battle>().GetSLGMap(); }
    }
    private GS_Battle _GameState
    {
        get { return GetGameStatus<GS_Battle>(); }
    }
    private GM_Battle _GameMode
    {
        get { return GetGameMode<GM_Battle>(); }
    }
    /// <summary>
    /// 控制一个AI对象，如果所有的对象都行动完毕找不到下一个则结束敌人的回合
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    private void PossessAI(int Index)
    {
        if (Index < _GameState.GetNumLocalEnemies())
        {
            UPawn Pawn = _GameState.GetLocalEnemyByIndex(Index);
            Possess(Pawn);
            ArrowOnCharacterTwoSeconds();
        }
        else //跳过控制结束回合
        {
            UnPossess();
            EndEnemyTurn();
        }
    }
    /// <summary>
    /// 结束敌人回合返回控制权到玩家手中
    /// </summary>
    private void EndEnemyTurn()
    {
        _GameMode.EndRound(ActivePlayerTurn);
    }
    /// <summary>
    /// 玩家开始行动
    /// </summary>
    private void ActivePlayerTurn()
    {
        BattleArrow.Reset();
    }
    /// <summary>
    /// 控制下一个对象
    /// </summary>
    public void PossessNext()
    {
        CurrentIndex++;
        Debug.Log("Possess Next:" + CurrentIndex);
        PossessAI(CurrentIndex);
    }
    /// <summary>
    /// 移动摄像机并显示光标到角色上1s
    /// </summary>
    private void ArrowOnCharacterTwoSeconds()
    {
        BattleArrow.SetArrowActive(true,AIPawnTilePos);
        Utils.GameUtil.DelayFunc(OnArrowShowFinish, 2f);
    }
    /// <summary>
    /// 隐藏箭头
    /// </summary>
    private void OnArrowShowFinish()
    {
        BattleArrow.SetArrowActive(false);
        if (CheckEnemyInMoveAtkRange() > 0)
            FindAttackMove();
        else
        {
            Debug.Log("没有可以攻击的对象");
        }
    }
    #region 检查行动函数   
    public void SetAI(int ai0, int ai1, int ai2)
    {
        ai[0] = ai0;
        ai[1] = ai1;
        ai[2] = ai2;
    }
    public int GetAI(int index)
    {
        return ai[index];
    }
    private bool IsNeedHeal()
    {
        switch (GetAI(2))
        {
            case (int)HEALPROMISE.DontHeal:
                return false;
            case (int)HEALPROMISE.HalfHP:
                if (CurrentControledPawn.GetCurrentHP() <= CurrentControledPawn.GetMaxHP() / 2)
                    return true;
                else
                    return false;
            case (int)HEALPROMISE.LesseHP:
                break;
            case (int)HEALPROMISE.OneThirdHP:
                if (CurrentControledPawn.GetCurrentHP() <= CurrentControledPawn.GetMaxHP() / 3)
                    return true;
                else
                    return false;
            default:
                return false;
        }
        return false;
    }
    private bool CanMove()
    {
        /*if (input.isHaveMapState(MapState.Sleep) || input.isHaveMapState(MapState.UnMove))
        {
            return false;
        }*/
        switch (GetAI(1))
        {
            case (int)AIACTION.DontMoveAttackInAttackRange:
                return false;
            case (int)AIACTION.DontMoveHeal:
                return false;
            case (int)AIACTION.Stay:
                return false;
            case (int)AIACTION.AfterAttackedAttack:
                if (CurrentControledPawn.GetCurrentHP() == CurrentControledPawn.GetMaxHP())
                    return false;
                else
                    return false;
            default:
                return true;
        }
    }
    /// <summary>
    /// 检查敌人是否在移动攻击范围内，敌人默认只有一个武器，只检查装备的武器
    /// 获取可攻击范围内的所有敌人，如果没有敌人则不进行操作，否则进行攻击目标的选择
    /// 将因己方单位占用而无法到达的区域也排除在外
    /// </summary>
    /// <returns></returns>
    private int CheckEnemyInMoveAtkRange()
    {
        InAtkRangeEnemyList.Clear();
        _Map.InitActionScope(CurrentControledPawn, false);
        GS_Battle _GS = GetGameStatus<GS_Battle>();
        for (int i = 0; i < _GS.GetNumLocalPlayers(); i++)
        {
            RPGCharacter Character = _GS.GetLocalPlayerByIndex(i) as RPGCharacter;
            Point2D TilePos = Character.GetTileCoord();
            if (_Map.IsCoordsAccessable(TilePos.x, TilePos.y))
            {
                InAtkRangeEnemyList.Add(Character);//将可以攻击到的角色添加到敌人可攻击列表
            }
        }
        return InAtkRangeEnemyList.Count;
    }
    /// <summary>
    /// 检查敌人是否在原地攻击范围内
    /// </summary>
    /// <returns></returns>
    private int CheckEnemyInAtkRange()
    {
        InAtkRangeEnemyList.Clear();
        List<Point2D> _attackRange = _Map.FindAttackRangeWithoutShow(CurrentControledPawn);
        GS_Battle _GS = GetGameStatus<GS_Battle>();
        for (int i = 0; i < _GS.GetNumLocalPlayers(); i++)
        {
            RPGCharacter Character = _GS.GetLocalPlayerByIndex(i) as RPGCharacter;
            Point2D TilePos = Character.GetTileCoord();
            if (_attackRange.Contains(new Point2D(TilePos.x, TilePos.y)))
            {
                InAtkRangeEnemyList.Add(Character);//将可以攻击到的角色添加到敌人可攻击列表
            }
        }
        return InAtkRangeEnemyList.Count;
    }
    #endregion

    #region 人物的AI行动策略
    /// <summary>
    /// 获得需要攻击的角色,若为null则表明虽然范围内有符合范围的，但是因己方单位占用导致人物无法移动到那里
    /// 进行移动到这个角色周围，先获取可以攻击到该人物的所有可到达图块，然后找到最有利的图块，进行攻击
    /// </summary>
    public void FindAttackMove()
    {
        Debug.Log("Find Move");
        RPGCharacter Character = CurrentControledPawn;
        Point2D AIPawnPos = Character.GetTileCoord();
        RPGCharacter enemy = SelectEnemyToAttack(Character);
        if (enemy == null) return;
        Point2D TileXY = GetSuitableTile(Character, enemy);
        if (TileXY == AIPawnTilePos)
        {//原地不用移动则直接攻击
            Debug.Log("不用移动,直接攻击");
            PossessNext();
        }
        else
        {
            _Map.MoveWithOutShowRoutine(CurrentControledPawn, TileXY.x, TileXY.y, () => { Debug.Log("AI移动结束，开始攻击"); PossessNext(); });
        }
    }
    public void DontMoveAttack()//不移动，直接原地攻击可攻击的目标
    {
        RPGCharacter enemy = SelectEnemyToAttack(CurrentControledPawn);//获得需要攻击的角色
    }
    public void MoveToNearest()// -         主动朝最近的角色进行移动
    {
        int minDistance = 100;
        GS_Battle _GS = GetGameStatus<GS_Battle>();
        for (int i = 0; i < _GS.GetNumLocalPlayers(); i++)
        {
            RPGCharacter Character = _GS.GetLocalPlayerByIndex(i) as RPGCharacter;
            Point2D TilePos = Character.GetTileCoord();
            int tempDistance = Point2D.GetDistance(Character.GetTileCoord(), CurrentControledPawn.GetTileCoord());
            if (tempDistance < minDistance)
            {
                minDistance = tempDistance;
                NearestEnemy = Character;//最近的敌人赋值给NearestEnemy
            }
        }
        if (NearestEnemy != null)
        {
            _Map.InitActionScope(CurrentControledPawn, false);
            minDistance = 100;
            Point2D Destine = new Point2D(0, 0);
            foreach (Point2D p in _Map.GetRealMoveableTiles())//Plist里记录的是真实可以移动到达的区域部分
            {
                int tempDistance = Point2D.GetDistance(p, NearestEnemy.GetTileCoord());
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    Destine.x = p.x;
                    Destine.y = p.y;
                }
            }
            // input.action_State = ActionState.AIACTION_MOVING;//若人物没有进行移动则会卡在该状态无法继续下去
            _Map.MoveWithOutShowRoutine(CurrentControledPawn, Destine.x, Destine.y, null);
        }
    }

    private void InitiativeMovetoLeader()// -         主动朝主角位置进行移动
    {
        currentAI = AIACTION.MoveAttackLeader;
    }
    private void InitiativeMovetoWeaker()// -         主动寻找地图上的虚弱单位
    {
        currentAI = AIACTION.MoveAttackWeaker;
    }
    private void InitiativeMovetoOnlyOneEnemyReach()//-         移动到我方只有一个人可以攻击的范围内
    {
        currentAI = AIACTION.MoveLessAccess;
    }
    private void InitiativeMovetoVillageOrTreasure()// -         主动破坏村庄或者偷取宝箱无视人物攻击，不进行主动攻击
    {
        currentAI = AIACTION.MoveVillageTreasure;
    }
    private void InitiativeFindAndHeal()// -         找寻范围内HP不足的单位进行治疗，或者攻击敌人（如果没有需要回复的单位且身上有攻击武器，则按照0进行移动）
    {
        currentAI = AIACTION.Heal;
    }
    private void DontMoveHeal()
    {
        currentAI = AIACTION.DontMoveHeal;
    }
    #endregion
    private void AttackWhenAvailable(RPGCharacter input)
    {
        currentAI = AIACTION.AttackInRange;
        //敌人在范围内，主动移动并攻击符合攻击ai部分的人物
        RPGCharacter enemy = SelectEnemyToAttack(input);//获得需要攻击的角色,若为null则表明虽然范围内有符合范围的，但是因己方单位占用导致人物无法移动到那里
        //进行移动到这个角色周围，先获取可以攻击到该人物的所有可到达图块，然后找到最有利的图块，进行攻击
        if (enemy == null) return;
        Point2D TileXY = GetSuitableTile(input, enemy);
        if (TileXY == input.GetTileCoord())//原地不用移动则直接攻击                      
        { //SLG.EnterBattle();
        }
        //input.action_State = ActionState.AIACTION_MOVING;//若人物没有进行移动则会卡在该状态无法继续下去
        _Map.MoveThenAttack(input, TileXY.x, TileXY.y, null, delegate
         {
             /*SLGLevel.SLG.RotateToEnemy(SLGLevel.SLG.getCurrentSelectGameChar(), SLGLevel.SLG.getCurrentSelectEnemy());
             SLGLevel.SLG.RotateToEnemy(SLGLevel.SLG.getCurrentSelectEnemy(), SLGLevel.SLG.getCurrentSelectGameChar());
             SLGLevel.SLG.EnterBattle();*/
         });//当前人物不进行移动时会出现卡死在Moving状态的bug，修改为不移动的话则直接进行攻击，不要此行代码，移动才需要这行代码，攻击函数在其他地方出发

    }
    public RPGCharacter SelectEnemyToAttack(RPGCharacter input)//选择可攻击的范围内的敌人进行攻击，需要进行坐标判定是否可以到达该敌人旁边
    {
        RPGCharacter retChar = null;
        if (InAtkRangeEnemyList.Count == 1)//只有一个人在可攻击的范围内
            return InAtkRangeEnemyList[0];
        int attackAI = GetAI(1);
        if (attackAI == (int)ATTACKPRIORITY.Leader)//找寻主角
        {
            foreach (RPGCharacter ch in InAtkRangeEnemyList)
            {
                if (ch.IsLeader())
                {
                    retChar = ch;
                }
            }
        }
        if (attackAI == (int)ATTACKPRIORITY.PossibilityToDie)//攻击可能致死
        {
            int AttackScore = 0;
            foreach (RPGCharacter ch in InAtkRangeEnemyList)
            {
                AttackScore = getAttackScore(input, ch);
                if (AttackScore >= ch.GetCurrentHP())
                {
                    retChar = ch;
                }
            }
        }
        if (attackAI == (int)ATTACKPRIORITY.MaxDamage || retChar == null)//最大伤害 或者 可能致死的人物不存在
        {
            int maxAttackScore = 0;
            int tempAttackScore = 0;
            foreach (RPGCharacter ch in InAtkRangeEnemyList)
            {
                tempAttackScore = getAttackScore(input, ch);
                if (tempAttackScore > maxAttackScore)
                {
                    maxAttackScore = tempAttackScore;
                    retChar = ch;
                }
            }
        }
        if (attackAI == (int)ATTACKPRIORITY.MinHP)//找寻人物hp最少的攻击
        {
            int minHP = 0;
            int tempHP = 0;
            foreach (RPGCharacter ch in InAtkRangeEnemyList)
            {
                tempHP = ch.GetCurrentHP();
                if (tempHP < minHP)
                {
                    minHP = tempHP;
                    retChar = ch;
                }
            }
        }
        if (attackAI == (int)ATTACKPRIORITY.MaxHitRate)//找寻回避值最低的攻击
        {
            int minAvoid = 0;
            int tempAvoid = 0;
            foreach (RPGCharacter ch in InAtkRangeEnemyList)
            {
                tempAvoid = ch.GetAvoid();
                if (tempAvoid < minAvoid)
                {
                    minAvoid = tempAvoid;
                    retChar = ch;
                }
            }
        }
        if (attackAI == (int)ATTACKPRIORITY.Nearest)//找寻最近的敌人攻击
        {
            int minDistance = 0;
            int tempDistance = 0;
            foreach (RPGCharacter ch in InAtkRangeEnemyList)
            {
                tempDistance = Point2D.GetDistance(ch.GetTileCoord(), input.GetTileCoord());
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    retChar = ch;
                }
            }
        }
        if (attackAI == (int)ATTACKPRIORITY.Random)//随机寻找敌人攻击
        {
            int r = Random.Range(0, InAtkRangeEnemyList.Count);
            retChar = InAtkRangeEnemyList[r];
        }
        return retChar;
    }


    private int getAttackScore(RPGCharacter attacker, RPGCharacter defender)//得到攻击的伤害
    {
        WeaponItem item = attacker.Item.GetEquipItem();
        if (item == null || item.IsValid())//没有装备武器或者武器耐久度为0
            return 0;
        return attacker.GetAttack() - (defender.GetPhysicalDefense() + defender.GetMagicalDefense()) / 2;
    }
    public Point2D GetSuitableTile(RPGCharacter attacker, RPGCharacter defender)//得到最适合的图块
    {
        //获取Map中可到达的图块数组，得到Gamechar周围的坐标，以自己的武器范围扩展在此范围内寻找回避+防御*10最大的图块
        int ex = defender.GetTileCoord().x;
        int ey = defender.GetTileCoord().y;
        _Map.FindAttackRange(ex, ey, attacker.Item.GetEquipItem().GetDefinition(), false);
        List<Point2D> AttackRangeList = _Map.GetAttackRangeData();//得到敌方人物在我方武器的范围，存储在_AttackRangeData
        int tileValue = 0, tempTileValue = 0;
        Point2D xy = new Point2D(-1, -1);
        for (int i = 0; i < AttackRangeList.Count; i++)//在人物可攻击的坐标内
        {
            if (_Map.CanMoveTo(AttackRangeList[i]))//如果可攻击的坐标在人物的移动范围内
            {
                tempTileValue = (_Map.GetMapPhysicalDefenseValue(ex, ey) + _Map.GetMapMagicalDefenseValue(ex, ey)) * 10 + _Map.GetMapAvoidValue(ex, ey);//计算图块优先级
                if (tempTileValue >= tileValue)
                {
                    tileValue = tempTileValue;
                    xy.x = AttackRangeList[i].x;
                    xy.y = AttackRangeList[i].y;
                }

            }
        }
        return xy;
    }
    public int getCanAtkWeaponId(RPGCharacter attacker, RPGCharacter defender)//通过计算两个人物的距离获取可使用的武器ID
    {
        Point2D Tile1 = attacker.GetTileCoord();
        Point2D Tile2 = defender.GetTileCoord();
        int i = Point2D.GetDistance(Tile1, Tile2);
        List<WeaponItem> attackItems = attacker.Item.GetAttackWeapon();
        for (int j = 0; j < attackItems.Count; j++)
        {
            WeaponItem localItem = attacker.Item.GetItem(j);
            if ((localItem.GetDefinition().RangeType.MaxSelectRange >= i) && (localItem.GetDefinition().RangeType.MinSelectRange <= i) && (localItem.IsValid()))
                return localItem.ID;
        }
        return -1;
    }
}
