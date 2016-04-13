using UnityEngine;
using System.Collections.Generic;

namespace Sequence
{
    [AddComponentMenu("Sequence/Add Character")]
    public class GetWeapon : SequenceEvent
    {
        [Tooltip("获得该武器的人物ID,如果是-1则代表当前行动的角色获得")]
        [Range(-1, 100)]
        public int PlayerID = -1;
        public int WeaponID = 0;
        public AudioClip GetAudio;
        public override void OnEnter()
        {
            SoundController.Instance.PlaySound(GetAudio);
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().ShowGetWeapon(WeaponID);
            Invoke("LogicGetWeapon", 2.5f);
        }
        public override string GetSummary()
        {
            return "Player:" + PlayerID + " 获得装备：" + WeaponID;
        }
        public void LogicGetWeapon()
        {
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().Hide();
            if (PlayerID == -1)
            {
                RPGCharacter Character = GetGameMode<UGameMode>().GetPlayerPawn<Pawn_BattleArrow>().SelectedCharacter;
                if (Character != null)
                {
                    Character.Item.AddWeapon(WeaponID, Continue);
                }
                else {
                    Debug.LogError("无法获取当前行动人物");
                    Continue();
                }
            }
            else
            {
                UGameInstance.Instance.GetGameState<GS_Battle>().GetPlayer(PlayerID).Item.AddWeapon(WeaponID, Continue);
            }
            Debug.Log(GetSummary());
        }

    }
}