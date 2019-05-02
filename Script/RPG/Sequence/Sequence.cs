using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Sequence
{
    [HierarchyIcon("Sequence.png", 1)]
    public class Sequence : MonoBehaviour
    {
        [ExecuteInEditMode]
        public enum ExecutionState
        {
            Idle,
            Executing,
        }
        [Tooltip("片段执行完毕执行的事件")]
        public UnityEngine.Events.UnityEvent OnFinish;

        [NonSerialized]
        public ExecutionState executionState;
        /// <summary>
        /// OnEnable时开始执行
        /// </summary>
        public bool ExecuteOnEnable = false;
        public bool Skipable = true;
        [HideInInspector]
        public int ItemId = -1;
        [Tooltip("The name of the Sequence")]
        public string SequenceName = "New Block";

        [TextArea(2, 5)]
        [Tooltip("Description text to display under the Sequence")]
        public string description = "";

        [Tooltip("事件执行完立即删除，请注意如果有数据依赖，该物体不能立即删除")]
        public bool DestroyWhenFinish;

        [Tooltip("延迟删除的时间")]
        public float TimeSpan;

        [HideInInspector]
        [System.NonSerialized]
        public SequenceEvent ActiveEvent;

        // Index of last command executed before the current one
        // -1 indicates no previous command
        [HideInInspector]
        [System.NonSerialized]
        public int previousActiveSequenceEventIndex = -1;

        [HideInInspector]
        [System.NonSerialized]
        public float executingIconTimer;
        /// <summary>
        /// 触发该事件的Event引用
        /// </summary>
        public EventInfoCollection.EventTypeBase EventRef;

        public List<SequenceEvent> commandList = new List<SequenceEvent>();

        protected int executionCount;

        /**
         * Duration of fade for executing icon displayed beside blocks & commands.
         */
        public const float executingIconFadeTime = 0.5f;

        /**
         * Controls the next command to execute in the block execution coroutine.
         */
        [NonSerialized]
        public int jumpToSequenceEventIndex = -1;

        protected virtual void Awake()
        {
            // Give each child command a reference back to its parent block
            // and tell each command its index in the list.
            int index = 0;
            executionState = ExecutionState.Idle;
            GetComponentsInChildren(commandList);
            foreach (SequenceEvent command in commandList)
            {
                if (command == null)
                {
                    continue;
                }

                command.RootSequence = this;
                command.commandIndex = index++;
            }
        }
        public void Reset()
        {
            Awake();
        }
        public void Clear()
        {
            foreach (var v in commandList)
            {
                DestroyImmediate(v.gameObject, false);
            }
        }
        void Update()
        {
            if (Skipable && GameMode.Instance.InputManager.GetStartInput())
                GameMode.Instance.UIManager.ScreenNormalToDark(1.0f, true, delegate { Stop(); OnFinish.Invoke(); });
        }

        public virtual bool IsExecuting()
        {
            return (executionState == ExecutionState.Executing);
        }

        public virtual int GetExecutionCount()
        {
            return executionCount;
        }
        private Coroutine coroutine;
        public virtual bool Execute(UnityAction onComplete = null)
        {
            if (executionState != ExecutionState.Idle)
            {
                return false;
            }
            executionCount++;
            coroutine = StartCoroutine(ExecuteBlock(onComplete));

            return true;
        }

        protected virtual IEnumerator ExecuteBlock(UnityAction onComplete = null)
        {
            executionState = ExecutionState.Executing;

            int i = 0;
            while (true)
            {
                // Executing commands specify the next command to skip to by setting jumpToSequenceEventIndex using SequenceEvent.Continue()
                if (jumpToSequenceEventIndex > -1)
                {
                    i = jumpToSequenceEventIndex;
                    jumpToSequenceEventIndex = -1;
                }

                // Skip disabled commands, comments and labels
                while (i < commandList.Count && (!commandList[i].gameObject.activeInHierarchy || !commandList[i].enabled))
                {
                    i = commandList[i].commandIndex + 1;
                }

                if (i >= commandList.Count)
                {
                    break;
                }

                // The previous active command is needed for if / else / else if commands
                if (ActiveEvent == null)
                {
                    previousActiveSequenceEventIndex = -1;
                }
                else
                {
                    previousActiveSequenceEventIndex = ActiveEvent.commandIndex;
                }

                SequenceEvent command = commandList[i];
                ActiveEvent = command;

                command.isExecuting = true;
                // This icon timer is managed by the FlowchartWindow class, but we also need to
                // set it here in case a command starts and finishes execution before the next window update.
                command.executingIconTimer = Time.realtimeSinceStartup + executingIconFadeTime;
                command.Execute();

                // Wait until the executing command sets another command to jump to via SequenceEvent.Continue()
                while (jumpToSequenceEventIndex == -1)
                {
                    yield return null;
                }

                command.isExecuting = false;
            }

            executionState = ExecutionState.Idle;
            ActiveEvent = null;

            if (onComplete != null)
            {
                onComplete();
            }
            if (DestroyWhenFinish)
            {
                Destroy(gameObject, TimeSpan);
            }
        }

        public virtual void Stop()
        {
            // Tell the executing command to stop immediately
            if (ActiveEvent != null)
            {
                ActiveEvent.isExecuting = false;
                ActiveEvent.OnStopExecuting();
            }

            // This will cause the execution loop to break on the next iteration
            jumpToSequenceEventIndex = int.MaxValue;
            if (coroutine != null) StopCoroutine(coroutine);
        }

        public virtual System.Type GetPreviousActiveSequenceEventType()
        {
            if (previousActiveSequenceEventIndex >= 0 &&
                previousActiveSequenceEventIndex < commandList.Count)
            {
                return commandList[previousActiveSequenceEventIndex].GetType();
            }

            return null;
        }

        public void PlaySequence()
        {
            Execute(OnFinish.Invoke);
        }
        void OnEnable()
        {
            if (ExecuteOnEnable)
                PlaySequence();
        }
    }
}
