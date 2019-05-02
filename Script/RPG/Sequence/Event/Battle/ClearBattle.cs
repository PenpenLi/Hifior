using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Clear Battle")]
    public class ClearBattle : SequenceEvent
    {
        public override void OnEnter()
        {
            gameMode.unitShower.Clear();
            gameMode.GridTileManager.UnloadMap();
            gameMode.ChapterManager.ClearBattle();
            gameMode.pathShower.Clear();
            gameMode.BattleManager.ChangeState(BattleManager.EBattleState.Idel);
            Destroy(gameMode.ChapterManager.Event.gameObject);
            ResourceManager.UnloadUnusedResource();
            Continue();
        }
        public override string GetSummary()
        {
            return "清楚UnitShower,unload map,清除战场";
        }
    }
}