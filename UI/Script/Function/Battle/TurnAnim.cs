using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TurnAnim : AbstractUI
    {
        private Text text;
        private bool isAnimEnd = false;
        protected override void Awake()
        {
            base.Awake();

            text = GetComponent<Text>();
            Hide();
        }
        public override void Show()
        {
           /* if (SLGLevel.SLG._whichturn == TURN_TYPE.Player)
                text.text = "回合" + SLGLevel.SLG._round + "\n" + "我方行动回合";
            if (SLGLevel.SLG._whichturn == TURN_TYPE.Enemy)
                text.text = "回合" + SLGLevel.SLG._round + "\n" + "敌方行动回合";

            if (SLGLevel.SLG._whichturn == TURN_TYPE.PlayerAlly)
                text.text = "回合" + SLGLevel.SLG._round + "\n" + "我方同盟行动回合";
            if (SLGLevel.SLG._whichturn == TURN_TYPE.EnemyAlly)
                text.text = "回合" + SLGLevel.SLG._round + "\n" + "敌方同盟行动回合";
                */
            gameObject.SetActive(true);
            StartCoroutine(Anim());
        }
        IEnumerator Anim()
        {
            GetComponent<Animation>().Play("turnAnimation");
            isAnimEnd = false;
            yield return new WaitForSeconds(2.0f);
            GetComponent<Animation>().Stop("turnAnimation");
            gameObject.SetActive(false);
            isAnimEnd = true;
        }
        public bool isEnd
        {
            get
            {
                return isAnimEnd;
            }
        }
    }
}