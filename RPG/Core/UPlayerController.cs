using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 无实体，仅处理事件，用于控制切换Pawn
/// </summary>
public class UPlayerController : UController
{
    /// <summary>
    /// 是否暂停，如果不可以暂停则执行 CanUnpauseDelegate
    /// </summary>
    /// <param name="bPause"></param>
    /// <param name="CanUnpauseDelegate"></param>
    /// <returns></returns>
    public virtual bool SetPause(bool bPause, UnityAction CanUnpauseDelegate) { return true; }

    /** Command to try to pause the game. */
    public virtual void Pause()
    {
    }

    /** Trys to set the player's name to the given name. */
    public virtual void SetName(string S)
    {
    }

    /** SwitchLevel to the given MapURL. */
    public virtual void SwitchLevel(string URL) { }
}
