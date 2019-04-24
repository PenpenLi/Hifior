using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
namespace Sequence
{
    [AddComponentMenu("Sequence/Get Money")]
    public class GetMoney : SequenceEvent
    {
        public int MoneyAmount = 0;
        public AudioClip GetAudio;
        public override void OnEnter()
        {
            SoundController.Instance.PlaySound(GetAudio);
            gameMode.UIManager.GetItemOrMoney.ShowGetMoney(MoneyAmount);
            Utils.GameUtil.DelayFunc(LogicGetMoney, ConstTable.CONST_SHOW_GET_ITEM_MONEY_TIME);
        }
        public override string GetSummary()
        {
            return " 获得金钱：" + MoneyAmount;
        }
        public void LogicGetMoney()
        {
            gameMode.UIManager.GetItemOrMoney.Hide();
            //AddMoneyAction(0, MoneyAmount);
            gameMode.ChapterManager.AddCurrentTeamMoney(MoneyAmount);
            Continue();
        }

    }
}