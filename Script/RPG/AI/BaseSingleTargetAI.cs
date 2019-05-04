using UnityEngine;
namespace RPG.AI
{
    public abstract class BaseSingleTargetAI : BaseAI
    {
        /// <summary>
        /// 找到想要攻击的目标单位
        /// </summary>
        /// <returns></returns>
        protected abstract RPGCharacter Target();
        public BaseSingleTargetAI(RPGCharacter ch) : base(ch)
        {
        }
        protected ETargetCamp targetCamp;
        public void SetTargetCamp(ETargetCamp _targetCamp) { targetCamp = _targetCamp; }

        public override void Action()
        {
            Debug.Log("开始AI行动");
            CameraFollow();
        }
    }
}