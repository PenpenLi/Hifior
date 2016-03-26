using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ActionStatePanel : IPanel
    {
        private bool[] _bShow= new bool[2] { false, false };
        private Image[] _image;
        private Text[] _text;

        protected override void Awake()
        {
            base.Awake();
            
            _image = GetComponentsInChildren<Image>();
            _text = GetComponentsInChildren<Text>();
            Hide();
        }
        public void Show(int pos, string text)
        {
            if (pos > 1)
            {
                Debug.Log("大于1");
                return;
            }
            _text[pos].text = text;
            if (_bShow[pos])
                return;
            _bShow[pos] = true;
            _image[pos].GetComponent<Animator>().Play("show");
        }
        public override void Hide()
        {
            base.Hide();

            for (int i = 0; i < _bShow.Length; i++)
            {
                if (_bShow[i])
                {
                    _image[i].GetComponent<Animator>().Play("hide");
                    _bShow[i] = false;
                }
            }
        }
        public void Hide(int pos)
        {
            if (_bShow[pos])
            {
                _image[pos].GetComponent<Animator>().Play("hide");
                _bShow[pos] = false;
            }
        }
        /*
        void Update()
        {
            if (Input.GetMouseButton(0))
                Show(0,"nihao");
            if (Input.GetMouseButton(1))
                Hide();
        }
         * */
    }
}