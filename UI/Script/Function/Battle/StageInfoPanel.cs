using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class StageInfoPanel : IPanel
    {
        public Text tClear;
        public Text tTurn;
        // Use this for initialization

        public void init(string clearInfo, int turn)
        {
            tClear.text = clearInfo;
            tTurn.text = turn.ToString();
        }
        protected override void Awake()
        {
            base.Awake();

            Hide();
        }
    }
}