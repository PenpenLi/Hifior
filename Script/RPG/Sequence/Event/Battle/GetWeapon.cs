using UnityEngine;
using System.Collections.Generic;

namespace Sequence
{
    [AddComponentMenu("Sequence/Get Weapon")]
    public class GetWeapon : SequenceEvent
    {
        [Tooltip("获得该武器的人物ID,如果是-1则代表当前行动的角色获得")]
        [Range(-1, 100)]
        public int PlayerID = -1;
        public int WeaponID = 0;
        public AudioClip GetAudio;
        public override void OnEnter()
        {
            SoundManage.Instance.PlaySound(GetAudio);
            gameMode.UIManager.GetItemOrMoney.ShowGetWeapon(WeaponID);
            Utils.GameUtil.DelayFunc(this, LogicGetWeapon, ConstTable.CONST_SHOW_GET_ITEM_MONEY_TIME);
        }

        public override string GetSummary()
        {
            return "Player:" + PlayerID + " 获得装备：" + WeaponID;
        }
        public void LogicGetWeapon()
        {
            gameMode.UIManager.GetItemOrMoney.Hide();
            if (PlayerID == -1)
            {
                gameMode.BattleManager.CurrentCharacterLogic.Info.Items.AddProp(WeaponID);
            }
            else
            {

            }
            Continue();
        }

    }
}