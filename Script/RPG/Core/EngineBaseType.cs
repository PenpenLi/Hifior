using System;
using System.Collections.Generic;
/// <summary>
///  If Disabled, this tick will not fire
///  If CoolingDown, this tick has an interval frequency that is being adhered to currently
/// </summary>
public enum ETickState
{
    Disabled,
    Enabled,
    CoolingDown
};
/// <summary>
/// 输入的动作状态
/// </summary>
public enum EInputActionType
{
    IE_Clicked,
    IE_Pressed,
    IE_Released
}
public enum ESceneState
{
    Movie,
    StartMenu,
    BigMap,
    Home,
    Battle,
    GameOver
}
public enum EBattleStatus
{
    剧情,
    回合动画, 
    选择角色,
    选定移动,
    选定动作,
    攻击过程,

}