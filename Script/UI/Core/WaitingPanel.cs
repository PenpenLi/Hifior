using UnityEngine.Events;
using System.Collections;
using UnityEngine;
namespace RPG.UI
{
    public class WaitingPanel : IPanel
    {
        public void Show(UnityAction FinishAction, float Time)
        {
            base.Show();
            RegisterHideEvent(FinishAction);
            Utils.GameUtil.DelayFunc(this, Hide, Time);
        }
        /// <summary>
        /// 等待直到某个函数返回true
        /// </summary>
        /// <param name="FinishAction"></param>
        /// <param name="WaitUntilFunc"></param>
        public void Show(UnityAction FinishAction, System.Func<bool> WaitUntilFunc)
        {
            base.Show();
            RegisterHideEvent(FinishAction);
            StartCoroutine(this.WaitUntil(WaitUntilFunc));
        }
        private IEnumerator WaitUntil(System.Func<bool> GetBoolFunc)
        {
            yield return new WaitUntil(GetBoolFunc);
            Hide();
        }
    }
}
