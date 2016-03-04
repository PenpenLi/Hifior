using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
public class SequenceEvent : MonoBehaviour
{
    [HideInInspector]
    public int ItemId = -1; // Invalid flowchart item id

    [HideInInspector]
    public string ErrorMessage = "";

    [HideInInspector]
    public int IndentLevel;

    [NonSerialized]
    public int commandIndex;

    /**
     * Set to true by the parent block while the command is executing.
     */
    [NonSerialized]
    public bool isExecuting;

    /**
     * Timer used to control appearance of executing icon in inspector.
     */
    [NonSerialized]
    public float executingIconTimer;

    /**
     * Reference to the Block object that this command belongs to.
     * This reference is only populated at runtime and in the editor when the 
     * block is selected.
     */
    [NonSerialized]
    public Sequence RootSequence;


    public virtual void Execute()
    {
        OnEnter();
    }

    public virtual void Continue()
    {
        // This is a noop if the Block has already been stopped
        if (isExecuting)
        {
            Continue(commandIndex + 1);
        }
    }

    public virtual void Continue(int nextCommandIndex)
    {
        OnExit();
        if (RootSequence != null)
        {
            RootSequence.jumpToSequenceEventIndex = nextCommandIndex;
        }
    }

    public virtual void StopParentBlock()
    {
        OnExit();
        if (RootSequence != null)
        {
            RootSequence.Stop();
        }
    }

    /**
     * Called when the parent block has been requested to stop executing, and
     * this command is the currently executing command.
     * Use this callback to terminate any asynchronous operations and 
     * cleanup state so that the command is ready to execute again later on.
     */
    public virtual void OnStopExecuting()
    { }
    
    public virtual void OnEnter()
    { }

    public virtual void OnExit()
    { }

    public virtual void OnReset()
    { }

    /*
    public virtual bool HasReference(Variable variable)
    {
        return false;
    }
    */
    public virtual string GetSummary()
    {
        return "";
    }

    public virtual string GetHelpText()
    {
        return "";
    }

    /**
     * This command starts a block of commands.
     */
    public virtual bool OpenBlock()
    {
        return false;
    }

    /**
     * This command ends a block of commands.
     */
    public virtual bool CloseBlock()
    {
        return false;
    }
    
    /**
     * Returns true if the specified property should be displayed in the inspector.
     * This is useful for hiding certain properties based on the value of another property.
     */
    public virtual bool IsPropertyVisible(string propertyName)
    {
        return true;
    }

    /**
     * Returns true if the specified property should be displayed as a reorderable list in the inspector.
     * This only applies for array properties and has no effect for non-array properties.
     */
    public virtual bool IsReorderableArray(string propertyName)
    {
        return false;
    }

}/*
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

}*/