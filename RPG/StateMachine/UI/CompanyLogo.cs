using UnityEngine;
using System.Collections;
using System;

namespace FSM
{
    public class CompanyLogo : FSMState
    {
        private const float TRANSITION_TIME = 2.0f;
        private float time = 0.0f;
        public CompanyLogo()
        {
            stateID = StateID.CompanyLogo;
        }
        public override void Act(UGameState Manager)
        {
            time += Time.deltaTime;
        }

        public override void Reason(UGameState Manager)
        {
            if (time > TRANSITION_TIME)
            {
                time = 0.0f;
                Manager.SetTransition(Transition.StartGameMenu);
            }
        }
        
        public override void DoBeforeLeaving(UGameState Manager)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public override void DoBeforeEntering(UGameState Manager)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}