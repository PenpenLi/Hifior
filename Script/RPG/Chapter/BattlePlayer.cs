using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BattlePlayer : ManagerBase
{
    public void AddUnitToMap(RPGCharacter p, Vector2Int tilePos)
    {
        var logic = p.Logic;
        Transform unit = gameMode.unitShower.AddUnit(p.GetCamp(), logic.GetName(), logic.GetStaySprites(), logic.GetMoveSprites(), tilePos);
        p.SetTransform(unit);
        logic.SetTileCoord(tilePos);
        var camp = p.GetCamp();
        if (camp == EnumCharacterCamp.Player)
        {
            chapterManager.AddPlayerToBattle(p);
            PositionMath.SetTilePlayerOccupied(tilePos);
        }
        if (camp == EnumCharacterCamp.Enemy)
        {
            chapterManager.AddEnemyToBattle(p);
            PositionMath.SetTileEnemyOccupied(tilePos);
        }
    }
    public void MoveUnitAfterAction(EnumCharacterCamp camp, Vector2Int srcPos, Vector2Int destPos, float speed, UnityAction onComplete)
    {
        List<Vector2Int> routine = PositionMath.GetMoveRoutine(destPos);
        gameMode.unitShower.MoveUnit(routine, onComplete, speed);
        PositionMath.ResetTileOccupyStatus(srcPos);

        if (camp == EnumCharacterCamp.Enemy)
            PositionMath.SetTileEnemyOccupied(destPos);
        else
            PositionMath.SetTilePlayerOccupied(destPos);
    }
    public void MoveUnitByRoutine(List<Vector2Int> routine, float speed, UnityAction onComplete)
    {
        var s = PositionMath.GetTileOccupyStatus(routine[0]);
        PositionMath.ResetTileOccupyStatus(routine[0]);
        PositionMath.SetOccupyStatus(routine[routine.Count - 1], s);
        gameMode.unitShower.MoveUnit(routine, onComplete, speed);
    }

    public void KillUnitAt(Vector2Int tilePos, float v, UnityAction onComplete, bool triggerDeadEvent = false)
    {
        RPGCharacter ch = chapterManager.GetCharacterFromCoord(tilePos);
        KillUnit(ch, v, onComplete, triggerDeadEvent);
        PositionMath.ResetTileOccupyStatus(tilePos);
    }
    public void KillUnit(int Id, float v, UnityAction onComplete, bool triggerDeadEvent = false)
    {
        var ch = chapterManager.GetCharacterFromID(Id);
        KillUnit(ch, v, onComplete, triggerDeadEvent);
        PositionMath.ResetTileOccupyStatus(ch.GetTileCoord());
    }

    public void KillUnit(RPGCharacter ch, float v, UnityAction onComplete, bool triggerDeadEvent = false)
    {
        chapterManager.RemoveCharacter(ch);
        PositionMath.ResetTileOccupyStatus(ch.GetTileCoord());
        if (triggerDeadEvent)
        {
            chapterManager.CheckEnemyDeadEvent(ch.Logic.GetID(), () => { gameMode.unitShower.DisappearUnit(ch.GetTileCoord(), v, onComplete); });
        }
        else
        {
            gameMode.unitShower.DisappearUnit(ch.GetTileCoord(), v, onComplete);
        }
    }
    public static void AssembleAttackSequenceEvent(System.Func<Sequence.AttackAnimation> atkFunc, CharacterLogic attacker, CharacterLogic defender)
    {
        List<BattleAttackInfo> attackInfo = BattleLogic.GetAttackInfo(attacker, defender);
        Debug.Log(Utils.TextUtil.GetListString(attackInfo));

        var atk = atkFunc();
        atk.AttackInfo = attackInfo[0];
        atk.IsLeft = false;
        atk.WaitTime = 0.3f;
        if (attackInfo.Count > 1)
        {
            var counterAtk = atkFunc();
            counterAtk.AttackInfo = attackInfo[1];
            counterAtk.IsLeft = true;
            counterAtk.WaitTime = 1.0f;
        }
        //计算处方向 然后在Unitshower里面转向并攻击，抖动
    }

    public void AttackUnit(CharacterLogic attacker, CharacterLogic defender, UnityAction onComplete)
    {
        //以Sequence的形式呈现战斗过程，
        gameMode.BeforePlaySequence();
        gameMode.ResetSequence("Attack");
        AssembleAttackSequenceEvent(gameMode.AddSequenceEvent<Sequence.AttackAnimation>, attacker, defender);

        gameMode.PlaySequence(onComplete);
        //计算处方向 然后在Unitshower里面转向并攻击，抖动
    }
}
