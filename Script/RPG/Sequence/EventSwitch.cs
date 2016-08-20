using UnityEngine.Events;
using System.IO;
using UnityEngine;
using System;

namespace Sequence
{
    /// <summary>
    /// 包含事件开关的Sequence,例如村庄访问检测某个人是否已经访问过，比如村庄除了特定人员访问其他的访问事件不一样。比如当前某个事件需要反复触发
    /// </summary>
    public class EventSwitch : Sequence
    {
        public uint Count;
        private bool Open;
        public EventSwitch( uint Count = 1)
        {
            Open = true;
            this.Count = Count;
        }
        protected void UseEventCount()
        {
            Count--;
            if (Count == 0)
            {
                Open = false;
            }
            else
            {
                Open = true;
            }
        }
        public bool AlreadyTrigged
        {
            get { return Open; }
        }
    }
}