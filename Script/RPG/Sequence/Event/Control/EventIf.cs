using UnityEngine;
using System;

namespace Sequence
{
    public enum EComparer
    {
        Bigger,
        BiggerEqual,
        Equal,
        LessEqual,
        Less
    }
    /// <summary>
    /// 包含事件开关的Sequence,例如村庄访问检测某个人是否已经访问过，比如村庄除了特定人员访问其他的访问事件不一样。比如当前某个事件需要反复触发
    /// </summary>
    public abstract class EventIf : SequenceEvent
    {
        public abstract bool IsTrue();
        public GameObject WhenTrue;
        public GameObject WhenFalse;
        public override void OnReset()
        {
            base.OnReset();
            WhenTrue.SetActive(true);
            WhenFalse.SetActive(true);
        }
        public override void OnEnter()
        {
            if (IsTrue())
            {
                WhenFalse.SetActive(false);
            }
            else
            {
                WhenTrue.SetActive(false);
            }
            Continue();
        }
        public static bool Compare(EComparer Comparer, int l,int r) 
        {
            switch (Comparer)
            {
                case EComparer.Bigger:
                    return l > r;
                case EComparer.BiggerEqual:
                    return l >= r;
                case EComparer.Equal:
                    return l == r;
                case EComparer.LessEqual:
                    return l <= r;
                case EComparer.Less:
                    return l < r;
            }
            return false;
        }
    }
}