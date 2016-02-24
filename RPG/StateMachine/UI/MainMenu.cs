using System;
using UnityEngine;
namespace FSM
{
    public class MainMenu : FSMState
    {
        public MainMenu()
        {
            stateID = StateID.StartGameMenu;
        }
        public override void Act(UGameState Manager)
        {
            Debug.Log(stateID.ToString() + " action");
        }

        public override void Reason(UGameState Manager)
        {

        }
        public override void DoBeforeEntering(UGameState Manager)
        {
            base.DoBeforeEntering(Manager);
        }
    }
}