using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum EGameplayTaskState
{
    Uninitialized,
    AwaitingActivation,
    Paused,
    Active,
    Finished
}
public enum ETaskResourceOverlapPolicy
{
    /** Pause overlapping same-priority tasks. */
    StartOnTop,
    /** Wait for other same-priority tasks to finish. */
    StartAtEnd,
};
public interface IGameplayTaskOwnerInterface
{
    void OnTaskInitialized(UGameplayTask Task);
    UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task);
    /// <summary>
    /// 任务开始或者激活的时候触发该事件
    /// </summary>
    /// <param name="Task"></param>
    void OnTaskActivated(UGameplayTask Task);
    /// <summary>
    /// 任务结束或被暂停的时候触发该事件
    /// </summary>
    /// <param name="Task"></param>
    void OnTaskDeactivated(UGameplayTask Task);
    UActor GetOwnerActor(UGameplayTask Task);
    UActor GetAvatarActor(UGameplayTask Task);
    int GetDefaultPriority();
}
public class UGameplayTask
{
    protected string InstanceName;

    /** This controls how this task will be treaded in relation to other, already running tasks, 
	 *	provided GameplayTasksComponent is configured to care about priorities (the default behavior)*/
    protected int Priority;

    protected EGameplayTaskState TaskState;

    protected ETaskResourceOverlapPolicy ResourceOverlapPolicy;

    /** If true, this task will receive TickTask calls from TasksComponent */
    protected bool bTickingTask;

    /** Should this task run on simulated clients? This should only be used in rare cases, such as movement tasks. Simulated Tasks do not broadcast their end delegates.  */
    protected bool bSimulatedTask;

    /** Am I actually running this as a simulated task. (This will be true on clients that simulating. This will be false on the server and the owning client) */
    protected bool bIsSimulating;

    protected bool bIsPausable;

    protected bool bCaresAboutPriority;

    /** this is set to avoid duplicate calls to task's owner and TasksComponent when both are the same object */
    protected bool bOwnedByTasksComponent;

    /// <summary>
    /// 包含该任务的Owner
    /// </summary>
    IGameplayTaskOwnerInterface TaskOwner;
    /// <summary>
    /// 任务组件
    /// </summary>
    UGameplayTasksComponent TasksComponent;

    public string GetInstanceName() { return InstanceName; }
    public bool IsTickingTask() { return bTickingTask; }
    public bool IsSimulatedTask() { return bSimulatedTask; }
    public bool IsSimulating() { return bIsSimulating; }
    public bool IsPausable() { return bIsPausable; }
    public int GetPriority() { return Priority; }
    public EGameplayTaskState GetState() { return TaskState; }
    public bool IsActive() { return (TaskState == EGameplayTaskState.Active); }

    public IGameplayTaskOwnerInterface GetTaskOwner() { return TaskOwner; }
    public UGameplayTasksComponent GetGameplayTasksComponent() { return TasksComponent; }

    bool IsOwnedByTasksComponent() { return bOwnedByTasksComponent; }

    public T NewTask<T>(IGameplayTaskOwnerInterface TaskOwner, string InstanceName) where T : UGameplayTask
    {
        T MyObj = new UGameplayTask() as T;
        MyObj.InstanceName = InstanceName;
        MyObj.InitTask(TaskOwner, TaskOwner.GetDefaultPriority());
        return MyObj;
    }

    protected void InitTask(IGameplayTaskOwnerInterface InTaskOwner, int InPriority)
    {
        Priority = InPriority;
        TaskOwner = InTaskOwner;
        UGameplayTasksComponent GTComponent = InTaskOwner.GetGameplayTasksComponent(this);
        TasksComponent = GTComponent;

        bOwnedByTasksComponent = (TaskOwner == GTComponent);

        TaskState = EGameplayTaskState.AwaitingActivation;

        InTaskOwner.OnTaskInitialized(this);
        if (bOwnedByTasksComponent == false && GTComponent != null)
        {
            GTComponent.OnTaskInitialized(this);
        }
    }
    public virtual void InitSimulatedTask(UGameplayTasksComponent InGameplayTasksComponent)
    {
        TasksComponent = InGameplayTasksComponent;
        bIsSimulating = true;
    }
    public void ReadyForActivation()
    {
        if (TasksComponent != null)
        {
            PerformActivation();
        }
        else
        {
            EndTask();
        }
    }
    public UGameInstance GetGameInstance()
    {
        Assert.IsNotNull(UGameInstance.Instance, "GameInstance 不能为Null");
        return UGameInstance.Instance;
    }

    public UActor GetOwnerActor()
    {
        if (TaskOwner != null)
        {
            return TaskOwner.GetOwnerActor(this);
        }
        else if (TasksComponent != null)
        {
            return TasksComponent.GetOwnerActor(this);
        }

        return null;
    }

    UActor GetAvatarActor()
    {
        if (TaskOwner != null)
        {
            return TaskOwner.GetAvatarActor(this);
        }
        else if (TasksComponent != null)
        {
            return TasksComponent.GetAvatarActor(this);
        }

        return null;
    }

    public void TaskOwnerEnded()
    {
        Debug.Log(GetGameplayTasksComponent() + "%s TaskOwnerEnded called, current State: %s");

        if (TaskState != EGameplayTaskState.Finished)
        {
            OnDestroy(true);
        }
    }

    public void EndTask()
    {
        Debug.Log(GetGameplayTasksComponent() + "%s EndTask called, current State: %s");

        if (TaskState != EGameplayTaskState.Finished)
        {
            OnDestroy(false);
        }
    }

    public virtual void ExternalConfirm(bool bEndTask)
    {
        Debug.Log(GetGameplayTasksComponent() + "%s ExternalConfirm called, bEndTask = %s, State : %s");

        if (bEndTask)
        {
            EndTask();
        }
    }

    public virtual void ExternalCancel()
    {
        Debug.Log(GetGameplayTasksComponent() + "%s ExternalCancel called, current State: %s");

        EndTask();
    }
    public void PerformActivation()
    {
        if (TaskState == EGameplayTaskState.Active)
        {
            Debug.Log(GetGameplayTasksComponent() + "%s PerformActivation called while TaskState is already Active. Bailing out.");
            return;
        }

        TaskState = EGameplayTaskState.Active;

        TasksComponent.OnTaskActivated(this);

        if (bOwnedByTasksComponent == false && TaskOwner != null)
        {
            TaskOwner.OnTaskActivated(this);
        }
    }

    public virtual void Pause()
    {
        Debug.Log(GetGameplayTasksComponent() + "%s Pause called, current State: %s");

        TaskState = EGameplayTaskState.Paused;

        TasksComponent.OnTaskDeactivated(this);

        if (bOwnedByTasksComponent == false && TaskOwner != null)
        {
            TaskOwner.OnTaskDeactivated(this);
        }
    }

    public virtual void Resume()
    {
        Debug.Log(GetGameplayTasksComponent() + "%s Resume called, current State: %s");

        TaskState = EGameplayTaskState.Active;

        TasksComponent.OnTaskActivated(this);

        if (bOwnedByTasksComponent == false && TaskOwner != null)
        {
            TaskOwner.OnTaskActivated(this);
        }
    }
    public void ActivateInTaskQueue()
    {
        switch (TaskState)
        {
            case EGameplayTaskState.Uninitialized:
                Debug.Log(GetGameplayTasksComponent() + "UGameplayTask::ActivateInTaskQueue Task %s passed for activation withouth having InitTask called on it!");
                break;
            case EGameplayTaskState.AwaitingActivation:
                PerformActivation();
                break;
            case EGameplayTaskState.Paused:
                // resume
                Resume();
                break;
            case EGameplayTaskState.Active:
                // nothing to do here
                break;
            case EGameplayTaskState.Finished:
                // If a task has finished, and it's being revived let's just treat the same as AwaitingActivation
                PerformActivation();
                break;
            default:
                break;
        }
    }

    public void PauseInTaskQueue()
    {
        switch (TaskState)
        {
            case EGameplayTaskState.Uninitialized:
                Debug.Log(GetGameplayTasksComponent() + "UGameplayTask::PauseInTaskQueue Task %s passed for pausing withouth having InitTask called on it!");
                break;
            case EGameplayTaskState.AwaitingActivation:
                // nothing to do here. Don't change the state to indicate this task has never been run before
                break;
            case EGameplayTaskState.Paused:
                // nothing to do here. Already paused
                break;
            case EGameplayTaskState.Active:
                // pause!
                Pause();
                break;
            case EGameplayTaskState.Finished:
                // nothing to do here. But sounds odd, so let's log this, just in case
                Debug.Log(GetGameplayTasksComponent() + "UGameplayTask::PauseInTaskQueue Task %s being pause while already marked as Finished");
                break;
            default:
                break;
        }
    }
    protected virtual void OnDestroy(bool bOwnerFinished)
    {
        Assert.IsTrue(TaskState != EGameplayTaskState.Finished);

        TaskState = EGameplayTaskState.Finished;

        // First of all notify the TaskComponent
        if (TasksComponent != null)
        {
            TasksComponent.OnTaskDeactivated(this);
        }

        // Remove ourselves from the owner's task list, if the owner isn't ending
        if (bOwnedByTasksComponent == false && bOwnerFinished == false && TaskOwner != null)
        {
            TaskOwner.OnTaskDeactivated(this);
        }
    }

#if UNITY_EDITOR
    public string GenerateDebugDescription()
    {
        return "";
    }

    public string GetTaskStateName()
    {
        return TaskState.ToString();
    }
#endif
}
public class UGameplayTasksComponent :UActor, IGameplayTaskOwnerInterface
{
    protected List<UGameplayTask> SimulatedTasks;
    protected List<UGameplayTask> TaskPriorityQueue;
    protected List<UGameplayTask> TickingTasks;
    protected int TopActivePriority;

    public bool bIsActive;
    #region IGamePlayTaskOwnerInterface
    public UActor GetAvatarActor(UGameplayTask Task)
    {
        throw new NotImplementedException();
    }

    public int GetDefaultPriority()
    {
        throw new NotImplementedException();
    }

    public UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task)
    {
        throw new NotImplementedException();
    }

    public UActor GetOwnerActor(UGameplayTask Task)
    {
        throw new NotImplementedException();
    }

    public void OnTaskInitialized(UGameplayTask Task)
    {
        throw new NotImplementedException();
    }

    public void OnTaskActivated(UGameplayTask Task)
    {
        if (Task.IsTickingTask())
        {
            Assert.IsTrue(TickingTasks.Contains(Task) == false);
            TickingTasks.Add(Task);

            // If this is our first ticking task, set this component as active so it begins ticking
            if (TickingTasks.Count == 1)
            {
                UpdateShouldTick();
            }
        }
        if (Task.IsSimulatedTask())
        {
            Assert.IsTrue(SimulatedTasks.Contains(Task) == false);
            SimulatedTasks.Add(Task);
        }
    }

    public void OnTaskDeactivated(UGameplayTask Task)
    {
        throw new NotImplementedException();
    }
    #endregion
    public void UpdateShouldTick()
    {
    }
}
