using UnityEngine.UI;

namespace RPG.UI
{
    public class HintsPanel : IPanel
    {
        public Text HitsHead;
        public Text Hitstext;
        public Button ConfirmButton;
        public bool isHide;

        protected override void Awake()
        {
            base.Awake();
            ConfirmButton.onClick.AddListener(Hide);
        }
        public override void Show()
        {
            base.Show();
            ConfirmButton.Select();
        }
        public void Show(string Hints)
        {
            Show();
            string[] s = Hints.Split('#');
            if (s.Length > 1)
            {
                HitsHead.text = s[0];
                Hitstext.text = s[1];
            }
            else
            {
                HitsHead.text = "提示";
                Hitstext.text = Hints;
            }
        }
        public void Show(string Caption, string Content)
        {
            Show();
            HitsHead.text = Caption;
            Hitstext.text = Content;
        }
    }
}