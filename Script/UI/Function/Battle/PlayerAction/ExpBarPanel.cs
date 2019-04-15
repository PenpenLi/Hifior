using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace RPG.UI
{
    public class ExpBarPanel : IPanel
    {
        public Image iExpBar;
        public Text tLV;
        public float BarWidth = 196.0f;
        private RectTransform rt;
        private int curExp;
        private int curLV = 1;
        public bool bExpShowFinish;

        protected override void Awake()
        {
            base.Awake();

            rt = iExpBar.GetComponent<RectTransform>();
            Hide();
        }
        public void Show(int curLV, int Exp, int boostExp)
        {
            bExpShowFinish = false;
            gameObject.SetActive(true);
            this.curLV = curLV;
            rt.sizeDelta = new Vector2((Exp / 100.0f) * BarWidth, rt.sizeDelta.y);
            tLV.text = curLV.ToString();
            StartCoroutine(BarFadeAdd(Exp, boostExp));
        }
        public void Show(RPGCharacter ch, int boostExp)
        {
            bExpShowFinish = false;
            gameObject.SetActive(true);
            this.curLV = ch.Logic.GetLevel();
            rt.sizeDelta = new Vector2((ch.Logic.GetExp() / 100.0f) * BarWidth, rt.sizeDelta.y);
            tLV.text = curLV.ToString();
            StartCoroutine(BarFadeAdd(ch.Logic.GetExp(), boostExp));
        }
        IEnumerator BarFadeAdd(int start, int boost)
        {
            yield return new WaitForSeconds(1.0f);
            curExp = start;
            for (int i = 0; i < boost; i++)
            {
                curExp++;
                if (curExp == 100)
                {
                    Debug.Log("升级了，提高一点HP");//LVUPpanel
                    curExp -= 100;
                    curLV++;
                    tLV.text = curLV.ToString();
                }
                rt.sizeDelta = new Vector2(((float)curExp / 100.0f) * BarWidth, rt.sizeDelta.y);
                yield return null;
            }
            yield return new WaitForSeconds(0.8f);
            bExpShowFinish = true;
            gameObject.SetActive(false);
        }
    }
}