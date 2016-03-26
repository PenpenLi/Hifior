using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.UI
{
    public class BattleHPPanel : IPanel
    {
        public bool bReducing;//是否正在HP条正在减少
        public Text textHP;
        public Text textName;
        public Image imageHPBar;
        int currentHP;
        int maxHP;
        int desHP;
        private RectTransform rBar;
        float barX, barY;

        protected override void Awake()
        {
            base.Awake();

            rBar = imageHPBar.GetComponent<RectTransform>();
            barX = rBar.sizeDelta.x;
            barY = rBar.sizeDelta.y;
        }

        void Update()
        {
            /*
            if (!bReducing)
            {
                if (Input.GetMouseButtonDown(0))
                    Change(-20);
                if (Input.GetMouseButtonDown(1))
                    Change(10);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    float per = (float)desHP / maxHP;
                    currentHP = desHP;
                    rBar.sizeDelta.Set(per * barX, barY);
                    textHP.text = currentHP.ToString();
                    bReducing = false;
                }
            }*/
        }
        public void Show(string name, int CurrentHP, int MaxHP)
        {
            textName.text = name;
            textHP.text = CurrentHP.ToString();
            currentHP = CurrentHP;
            maxHP = MaxHP;
            float per = (float)CurrentHP / MaxHP;
            rBar.sizeDelta = new Vector2(per * barX, barY);
        }
        public void Change(int ChangedHP, float delayTime = 0.0f)
        {
            bReducing = true;//指示正在减少
            StartCoroutine(Changing(ChangedHP, delayTime));
        }
        IEnumerator Changing(int change, float delay)
        {
            yield return new WaitForSeconds(delay);
            desHP = currentHP + change;
            if (desHP < 0) desHP = 0;
            if (desHP > maxHP) desHP = maxHP;
            float _time = 0.0f;
            int absChange = Mathf.Abs(change);
            if (absChange < 5)
                _time = 0.2f / absChange;
            else if (absChange < 10)
                _time = 0.4f / absChange;
            else if (absChange < 20)
                _time = 0.6f / absChange;
            else
                _time = 0.8f / absChange;
            while (currentHP != desHP)
            {
                if (desHP < currentHP)
                    currentHP--;
                else
                    currentHP++;
                float per = (float)currentHP / maxHP;
                rBar.sizeDelta = new Vector2(per * barX, barY);
                textHP.text = currentHP.ToString();
                yield return new WaitForSeconds(_time);
            }
            bReducing = false;
        }
    }
}