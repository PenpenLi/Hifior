using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace RPG.UI
{
    public class UI_BattleAttackInfo : IPanel
    {
        public Image PlayerIcon;
        public Image WeaponIcon;
        public Text WeaponName;
        public Text HP;
        public Text HIT;
        public Text DMG;
        public Text CRT;
        public Text AfterDamageHPValue;
        public Image MaxHPImage;
        public Image CurHPImage;
        public Image AfterDamageHPImage;
        private Color curHPColor;
        private Vector2 hpBarSize;
        protected override void Awake()
        {
            base.Awake();
            hpBarSize = MaxHPImage.rectTransform.sizeDelta;
            curHPColor = CurHPImage.color;
        }

        private Vector2 getHPbarSize(int hp, int maxHP)
        {
            return new Vector2((float)(hp) / maxHP * hpBarSize.x, hpBarSize.y);
        }
        public void Show(Sprite player_icon, Sprite weapon_icon, string weapon_name, int maxHP, int curHP, int afterHP, int hit, int dmg, int crt, int dmgCount)
        {
            PlayerIcon.sprite = player_icon;
            WeaponIcon.sprite = weapon_icon;
            WeaponName.text = weapon_name;
            HP.text = curHP.ToString();
            HIT.text = "HIT    " + hit.ToString();
            DMG.text = "DMG   " + dmg.ToString() + (dmgCount == 1 ? "" : " x " + dmgCount);
            CRT.text = "CRT    " + crt.ToString();
            if (afterHP < 0) afterHP = 0;
            AfterDamageHPValue.text = afterHP.ToString();
            MaxHPImage.rectTransform.sizeDelta = hpBarSize;
            CurHPImage.rectTransform.sizeDelta = getHPbarSize(curHP, maxHP);
            AfterDamageHPImage.rectTransform.sizeDelta = getHPbarSize(afterHP, maxHP);
            base.Show();
        }
        public void SetHP(int max, int src, int dest, int speed, float waitTime, UnityAction onComplete = null)
        {
            StartCoroutine(ISetHPBar(max, src, dest, speed, waitTime, onComplete));
        }
        private void Update()
        {
            CurHPImage.color = new Color(curHPColor.r, curHPColor.g, curHPColor.b, 0.3f + Mathf.PingPong(Time.time, 0.7f));
        }
        IEnumerator ISetHPBar(int max, int src, int dest, int speed, float waitTime, UnityAction onComplete)
        {
            float srcR = (float)src / max;
            float destR = (float)dest / max;
            var curRt = CurHPImage.GetComponent<RectTransform>();
            var aftRt = AfterDamageHPImage.GetComponent<RectTransform>();
            for (int i = 0; i < Application.targetFrameRate; i += speed)
            {
                float x = Mathf.Lerp(srcR, destR, (float)i / Application.targetFrameRate);
                aftRt.sizeDelta = new Vector2(hpBarSize.x * x, hpBarSize.y);
                if (curRt.sizeDelta.x > aftRt.sizeDelta.x)
                    curRt.sizeDelta = aftRt.sizeDelta;
                yield return null;
            }
            curRt.sizeDelta = new Vector2(hpBarSize.x * destR, hpBarSize.y);
            aftRt.sizeDelta = curRt.sizeDelta;
            yield return new WaitForSeconds(waitTime);
            onComplete?.Invoke();
        }
    }
}