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
        /// <summary>
        /// 第一个int参数是所属的队伍，因为游戏可能会出现分散的队伍
        /// </summary>
        public UnityAction<int,int> AddMoneyAction;
        public override void OnEnter()
        {
            SoundController.Instance.PlaySound(GetAudio);
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().ShowGetMoney(MoneyAmount);
            Invoke("LogicGetWeapon", 2.5f);
        }
        public override string GetSummary()
        {
            return " 获得金钱：" + MoneyAmount;
        }
        public void LogicGetWeapon()
        {
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().Hide();
            AddMoneyAction(0,MoneyAmount);
            Continue();
            Debug.Log(GetSummary());
        }

    }
}