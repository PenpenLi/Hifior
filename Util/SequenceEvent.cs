using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;
public class SequenceEvent
{
    /// <summary>
    /// 是否是自动结束的事件
    /// </summary>
    public bool bAutoFinish;
    /// <summary>
    /// 自动结束距离启动的时间
    /// </summary>
    public float AutoFinishTime;
    /// <summary>
    /// 是否在执行该事件
    /// </summary>
    public bool bRunning;
    /// <summary>
    /// 该事件是否已经结束
    /// </summary>
    public bool bFinish;
    public UnityAction ActionDelegate;
    public SequenceEvent(UnityAction InAction)
    {
        Assert.IsNotNull(InAction, "The action is null");
        ActionDelegate = InAction;
    }

}