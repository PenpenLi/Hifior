using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SmallTipUp : IPanel
    {
        public Text tTip;

        public void Show(Vector3 vector, string s)//解析#判定出现的 位置和内容
        {
            transform.position = vector;
            tTip.text = s;
            gameObject.SetActive(true);
        }
        protected override void Awake()
        {
            base.Awake();

            Hide();
        }
        /*
        public void Show(string s)//解析#判定出现的 位置和内容
        {
            string[] sp=s.Split('#');
            if (sp.Length < 3) return;
            float x = Convert.ToSingle(sp[0]);
            float y = Convert.ToSingle(sp[1]);
            transform.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0.0f);
            tTip.text = sp[3];
            gameObject.SetActive(true);
        }*/
    }
}