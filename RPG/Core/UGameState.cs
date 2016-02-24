using UnityEngine;
using System.Collections;
using FSM;
/// <summary>
/// 保存当前GameMode下的一些数据
/// </summary>
public class UGameState : UActor
{
    public GameObject CurrentPlayer { private set; get; }//当前操作的角色
    public string CurrentStateOnInspector;
    private FSMSystem fsm = new FSMSystem();//内置一个fsm
    public void SetTickInterval(float Interval = 0.2f)
    {
        Time.fixedDeltaTime = Interval;
    }
    public void SetTransition(Transition t) //转换状态
    {
        fsm.PerformTransition(t);
        CurrentStateOnInspector = t.ToString();
    }
    public void AddState(FSMState State, Transition InTransition, StateID InStateID)
    {
        State.AddTransition(InTransition, InStateID);
        fsm.AddState(State);
    }
    /// <summary>
    /// 重写这个函数用于创建一个状态机
    /// </summary>
    public virtual void MakeFSM()
    {
        CompanyLogo companyLogo = new CompanyLogo();
        AddState(companyLogo, Transition.StartGameMenu, StateID.StartGameMenu);

        MainMenu mainMenu = new MainMenu();
        AddState(mainMenu, Transition.ShowStartMovie, StateID.StartMovie);
    }
    public override void BeginPlay()
    {
        base.BeginPlay();

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
