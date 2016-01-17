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
        public override void Act(FSMManage Manager)
        {
            Debug.Log(stateID.ToString() + " action");
        }

        public override void Reason(FSMManage Manager)
        {

        }
        public override void DoBeforeEntering(FSMManage Manager)
        {
            base.DoBeforeEntering(Manager);
        }
    }
}