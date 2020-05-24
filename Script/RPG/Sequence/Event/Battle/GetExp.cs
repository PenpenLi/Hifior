using UnityEngine;
using System.Collections.Generic;
using RPG.UI;
namespace Sequence
{
    [AddComponentMenu("Sequence/Get Exp")]
    public class GetExp : SequenceEvent
    {
        public LevelUPInfo Info;
        public int PlayerID;
        public CharacterLogic Logic;
        public override void OnEnter()
        {
            gameMode.UIManager.BattleExpGain.RegisterHideEvent(Continue);
            gameMode.UIManager.BattleExpGain.Show(Info);
        }
        public override void Continue()
        {
            gameMode.UIManager.GetItemOrMoney.Hide();
            if (PlayerID == -1) Logic = gameMode.BattleManager.CurrentCharacterLogic;
            else Logic = gameMode.ChapterManager.GetCharacterFromID(PlayerID).Logic;
            Logic.SetLevel(Info.endLevel);
            Logic.SetExp(Info.endExp);
            base.Continue();
        }

        public override string GetSummary()
        {
            return "获得EXP";
        }
    }
}