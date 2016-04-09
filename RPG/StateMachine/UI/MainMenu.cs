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
        public override void Act(UGameStatus Manager)
        {
            Debug.Log(stateID.ToString() + " action");
        }

        public override void Reason(UGameStatus Manager)
        {

        }
        public override void DoBeforeEntering(UGameStatus Manager)
        {
            base.DoBeforeEntering(Manager);
        }
    }
}