﻿using UnityEngine;
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
            SoundController.Instance.PlaySound(GetAudio);
            UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().ShowGetProps(PropID);
            Utils.GameUtil.DelayFunc(this, LogicGetProps, 2.0f);
        }
        private void WaitSecondsToContinue()
        {
            Utils.GameUtil.DelayFunc(this, Continue, 1f);
        }
        public override string GetSummary()
        {
            return "Player:" + PlayerID + " 获得道具：" + PropID;
        }
        public void LogicGetProps()
        {
            //UIController.Instance.GetUI<RPG.UI.GetItemOrMoney>().Hide();
            //if (PlayerID == -1)
            //{
            //    RPGCharacter Character = GetGameMode<UGameMode>().GetPlayerPawn<Pawn_BattleArrow>().SelectedCharacter;
            //    if (Character != null)
            //    {
            //        Character.Item.AddProp(PropID, WaitSecondsToContinue);
            //    }
            //    else
            //    {
            //        Debug.LogError("无法获取当前行动人物");
            //        Continue();
            //    }
            //}
            //else
            //{
            //    UGameInstance.Instance.GetGameState<GS_Battle>().GetPlayer(PlayerID).Item.AddProp(PropID, WaitSecondsToContinue);
            //}
            //Debug.Log(GetSummary());
        }

    }
}