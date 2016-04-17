using UnityEngine;
using System.Collections.Generic;

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
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().ShowGetMoney(MoneyAmount);
            Invoke("LogicGetWeapon", 2.5f);
        }
        public override string GetSummary()
        {
            return  " 获得金钱：" + MoneyAmount;
        }
        public void LogicGetWeapon()
        {
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().Hide();
            UGameInstance.Instance.Ware.AddMoney(MoneyAmount);
            Continue();
            Debug.Log(GetSummary());
        }

    }
}