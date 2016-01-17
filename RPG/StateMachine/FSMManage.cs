using UnityEngine;
using System.Collections.Generic;
using FSM;
public class FSMManage : MonoSingleton<FSMManage>
{
    public GameObject CurrentPlayer { private set; get; }//当前操作的角色
    public string CurrentStateOnInspector;
    private FSMSystem fsm = new FSMSystem();//内置一个fsm

    public void SetTransition(Transition t) //转换状态
    {
        fsm.PerformTransition(t);
        CurrentStateOnInspector= t.ToString();
    }

    private void MakeFSM()
    {
        CompanyLogo companyLogo = new CompanyLogo();
        companyLogo.AddTransition(Transition.StartGameMenu, StateID.StartGameMenu);

        MainMenu mainMenu = new MainMenu();
        mainMenu.AddTransition(Transition.ShowStartMovie, StateID.StartMovie);

        fsm.AddState(companyLogo);
        fsm.AddState(mainMenu);
    }
    public void Start()
    {
        MakeFSM();
    }
    /// <summary>
    /// //作为驱动源
    /// </summary>
    public void FixedUpdate()
    {
        fsm.CurrentState.Reason(this);
        fsm.CurrentState.Act(this);
    }

}
