using UnityEngine;
using System.Collections.Generic;

namespace Sequence
{
    [AddComponentMenu("Sequence/Get Props")]
    public class GetProps : SequenceEvent
    {
        [Tooltip("获得该武器的人物ID,如果是-1则代表当前行动的角色获得")]
        [Range(-1, 100)]
        public int PlayerID = -1;
        public int PropID = 0;
        public AudioClip GetAudio;
        public override void OnEnter()
        {
            SoundManage.Instance.PlaySound(GetAudio);
            gameMode.UIManager.GetItemOrMoney.ShowGetProps(PropID);
            Utils.GameUtil.DelayFunc(this, LogicGetProps, ConstTable.CONST_SHOW_GET_ITEM_MONEY_TIME);
        }

        public override string GetSummary()
        {
            return "Player:" + PlayerID + " 获得道具：" + PropID;
        }
        public void LogicGetProps()
        {
            gameMode.UIManager.GetItemOrMoney.Hide();
            if (PlayerID == -1)
            {
                gameMode.BattleManager.CurrentCharacterLogic.Info.Items.AddProp(PropID);
            }
            else
            {

            }
            Continue();
        }

    }
}