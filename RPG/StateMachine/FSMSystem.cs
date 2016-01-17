using UnityEngine;
using System.Collections.Generic;
namespace FSM
{
    public enum Transition
    {
        NullTransition = 0,
        Awake,
        ShowStartMovie,
        StartGameMenu,
        LoadScene
    }
    public enum StateID
    {
        NullState = 0,
        CompanyLogo,
        StartMovie,
        StartGameMenu,
        Loading
    }
    public abstract class FSMState
    {
        protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
        protected StateID stateID;
        /// <summary>
        /// 当前的状态ID
        /// </summary>
        public StateID ID { get { return stateID; } }
        /// <summary>
        /// 添加转换节点
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="id"></param>
        public void AddTransition(Transition trans, StateID id)
        {
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("FSM State Error:NullTransition is not allowed for a real transition");
                return;
            }
            if (id == StateID.NullState)
            {
                Debug.LogError("FSM State Errot:NullStateID is not allowed for a real ID");
                return;
            }
            if (map.ContainsKey(trans))
            {
                Debug.LogError("FSM State Error:State " + stateID.ToString() + "already has transition " + trans.ToString() + "Impossible to assign to another state");
                return;
            }
            map.Add(trans, id);
        }
        /// <summary>
        /// 删除转换节点
        /// </summary>
        /// <param name="trans"></param>
        public void DeleteTransition(Transition trans)
        {
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("FSMState ERROR: NullTransition is not allowed");
                return;
            }
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);
                return;
            }
            Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                           " was not on the state's transition list");
        }
        /// <summary>
        /// 获取输出的状态
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public StateID GetOutputState(Transition trans)//此函数由下面这个脚本FSMSystem.cs中的PerformTransition函数调用。是用来检索状态的。
        {
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }
            return StateID.NullState;
        }
        public virtual void DoBeforeEntering(FSMManage Manager) { }
        public virtual void DoBeforeLeaving(FSMManage Manager) { }
        public abstract void Reason(FSMManage Manager);

        public abstract void Act(FSMManage Manager);
    }

    public class FSMSystem
    {
        private List<FSMState> states;
        private StateID currentStateID;
        /// <summary>
        /// 返回当前状态的ID
        /// </summary>
        public StateID CurrentStateID
        {
            get
            {
                return currentStateID;
            }
        }
        private FSMState currentState;
        /// <summary>
        /// 返回当前的状态
        /// </summary>
        public FSMState CurrentState
        {
            get
            {
                return currentState;
            }
        }
        public FSMSystem()
        {
            states = new List<FSMState>();
        }
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="s"></param>
        public void AddState(FSMState s)
        {
            if (s == null)
            {
                Debug.LogError("FSM ERROR: Null reference is not allowed");
            }

            if (states.Count == 0)/*第一次添加时必定执行这块代码，因为一开始states是空的，并且这块代码设置了第一次添加的状态是
默认的当前状态。这一点读者一定要理解，不然对于后面的东西读者会非常困惑的，因为其他地方没有地方设置运行后默认的当前状态。*/

            {
                states.Add(s);
                currentState = s;
                currentStateID = s.ID;//这里实例化了这两个成员变量
                return;
            }

            foreach (FSMState state in states)//排除相同的状态
            {
                if (state.ID == s.ID)
                {
                    Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() + " because state has already been added");
                    return;
                }
            }
            states.Add(s);//这一句代码第一次不执行，因为第一次states是空的，执行到上面的if里面后立即返回了
        }

        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="id"></param>
        public void DeleteState(StateID id)//跟据ID来从容器states中定向移除FSMState实例
        {
            if (id == StateID.NullState)
            {
                Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
                return;
            }
            foreach (FSMState state in states)
            {
                if (state.ID == id)
                {
                    states.Remove(state);
                    return;
                }
            }
            Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() + ". It was not on the list of states");
        }
        /// <summary>
        /// 执行状态转换
        /// </summary>
        /// <param name="trans"></param>
        public void PerformTransition(Transition trans)
        {
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            }

            StateID id = currentState.GetOutputState(trans);//这下我们得回到当初我所说讲到的FSMState.cs中的那个检索状态的函数。如果检索不出来，就返回NullStateId，即执行下面if语句。
            if (id == StateID.NullState)
            {
                Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " + " for transition " + trans.ToString());
                return;
            }
            currentStateID = id;//还是那句话，如果查到了有这个状态，那么我们就将其赋值给成员变量currentStateID。

            foreach (FSMState state in states)//遍历此状态容器
            {
                if (state.ID == currentStateID)
                {
                    currentState.DoBeforeLeaving(FSMManage.Instance);//我们在转换之前或许要做点什么吧！，所以我们如有需要，得在FSMState实现类中覆写一下这个方法
                    currentState = state;//好了，做完了转换之前的预备工作（DoBeforeLeaving），是时候该转换状态了
                    currentState.DoBeforeEntering(FSMManage.Instance);//状态转换完成之后，有可能得先为新状态做点事吧，那么我们也得DoBeforeEntering函数
                    break;
                }
            }

        }
    }
}
