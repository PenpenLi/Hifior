using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ItemTip : IPanel
    {

        public Text tTip;
        public float upSpace = 8f;
        public float bottomSpace = 8f;
        public float leftSpace = 10f;
        public float rightSpace = 10f;
        public float lineSpace = 30f;//单行文字占用的高度
        private RectTransform rt;
        protected override void Awake()
        {
            base.Awake();

            rt = gameObject.GetComponent<RectTransform>();
        }

        public void Show(Vector3 vector, string s, bool bUp = false)
        {
            if (bUp) rt.pivot = new Vector2(0.5f, -1.15f);
            else rt.pivot = new Vector2(0.5f, 1.15f);
            transform.position = vector;
            int line = s.Split('\n').Length;
            float height = upSpace + bottomSpace + lineSpace * line;
            float width = rt.sizeDelta.x;
            rt.sizeDelta = new Vector2(width, height);
            tTip.text = s;
            gameObject.SetActive(true);
        }
    }
}