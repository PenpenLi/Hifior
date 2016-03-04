/* 
 * 作者：杨太彦
 * 更新：2015-11-28
 * 描述：
 *      如果你使用过NGUI中的TWeen组件，那么想必对这个
 *      也相当熟悉了，这个就是用来控制UI组件实现一系列
 *      动画效果的，跟NGUI一样，里面提供了三种模式，分
 *      别是单次(Once)、循环(Loop)、巡回(Pingpong)。另
 *      外，你可以通过onFinish函数从编辑器或者动态为它
 *      添加回调事件，特别注意的是，回调事件只在单次(Once)
 *      模式下生效。现在它已经有很多派生类可以满足你的
 *      基本需求，如果你感觉那些还不够的话，那么你可以
 *      从它派生并实现UpdateTween(float _val)函数，来
 *      达到你预期的效果。详情，请看该函数的摘些。
 */
namespace TGE.Widget
{
    using UnityEngine;
    using UnityEngine.Events;
     
    public abstract class TGEUITween : MonoBehaviour
    {
        public enum EMode 
        {
            Once,
            Loop,
            Pingpong,
        }

        /// <summary>
        /// 使用的方式
        /// </summary>
        public EMode mode = EMode.Once;

        /// <summary>
        /// 多长时间完成
        /// </summary>
        public float timeConsuming = 1f;

        /// <summary>
        /// 在执行完成后调用此事件,该事件只在Once时有效
        /// </summary>
        public UnityEvent onFinish = null;

        public bool isForward { get { return way != -1; } }

        /// <summary>
        /// 执行的方向
        /// </summary>
        private int way = 1;

        /// <summary>
        /// 当前的时间
        /// </summary>
        private float timeCurrent  = 0f;

        protected virtual void Awake() 
        {
            timeConsuming = timeConsuming <= 0 ? 1 : timeConsuming;
        }

        protected virtual void Update() 
        {
            timeCurrent += Time.deltaTime * way;

            switch (mode) 
            {
            case EMode.Once:
                {
                    bool isOver = true;
                    if (way > 0 && timeCurrent > timeConsuming)
                        timeCurrent = timeConsuming;
                    else if (way < 0 && timeCurrent < 0)
                        timeCurrent = 0;
                    else isOver = false;

                    if (isOver) 
                    {
                        UpdateTween(timeCurrent / timeConsuming);
                        enabled = false;
                        if (onFinish != null)
                            onFinish.Invoke();
                    }
                } 
                break;
            case EMode.Loop:
                timeCurrent = Mathf.Repeat(timeCurrent < 0 ? Mathf.Abs(timeCurrent) : timeCurrent, timeConsuming); 
                break;
            case EMode.Pingpong:
                {
                    float nextTime = Mathf.Repeat(timeCurrent < 0 ? Mathf.Abs(timeCurrent) : timeCurrent, timeConsuming);
                    if (way > 0 && nextTime < timeCurrent) 
                    {
                        timeCurrent = timeConsuming;
                        way         = -1;
                    }
                    else if (way < 0 && nextTime > timeCurrent) 
                    {
                        timeCurrent = 0;
                        way         = 1;
                    }
                }
                break;
            }

            UpdateTween(timeCurrent / timeConsuming);
        }
        
        /// <summary>
        /// 设置到开始时的样子，如果你用的是once模式
        /// 那么需要手动调用Forward或者Reverse来启动它
        /// </summary>
        public void ResetBeginning() 
        {
            way         = 1;
            timeCurrent = 0;
            UpdateTween(0);

            if (mode == EMode.Once) 
                enabled = false;
        }

        /// <summary>
        /// 无论在那种模式中你都可以调用Forward
        /// 如果你使用once模式，那么在脚本没有
        /// 启动的情况下，它会帮你启动，并且会
        /// 初始化到起点，开始前进。
        /// </summary>
        public void Forward() { ResetWay(1); }

        /// <summary>
        /// 无论在那种模式中你都可以调用Reverse
        /// 如果你使用once模式，那么在脚本没有
        /// 启动的情况下，它会帮你启动，并且会
        /// 初始化到终点，开始返回。
        /// </summary>
        public void Reverse() { ResetWay(-1); }
        
        private void ResetWay(int _way) 
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true); 

            way = _way;

            if (mode == EMode.Once && enabled == false)
            {
                timeCurrent = _way > 0 ? 0 : timeConsuming; 
                UpdateTween(timeCurrent / timeConsuming);
                enabled = true;
            } 
            
        }

        /// <summary>
        /// 如果你想扩展其他的差值类型，可以继承此类重写UpdateTween
        /// 函数，该函数只接受一个0~1的区间值，方便你对需要的东西进行
        /// 编排。
        /// </summary>
        /// <param name="_radio"></param>
        protected abstract void UpdateTween(float _radio);
    }
}
