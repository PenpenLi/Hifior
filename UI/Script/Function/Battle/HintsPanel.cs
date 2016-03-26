using UnityEngine.UI;

namespace RPG.UI
{
    public class HintsPanel : IPanel
    {
        public Text HitsHead;
        public Text Hitstext;
        public bool isHide;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(string sHits)
        { //如果包含标题
            string[] s = sHits.Split('#');
            if (s.Length > 1)
            {
                HitsHead.text = s[0];
                Hitstext.text = s[1];
            }
            else
            {
                HitsHead.text = "提示";
                Hitstext.text = sHits;
            }
            gameObject.SetActive(true);
        }
    }
}