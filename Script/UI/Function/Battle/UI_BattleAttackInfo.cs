using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        private Vector2 hpBarSize;
        public override void BeginPlay()
        {
            base.BeginPlay();
            hpBarSize = MaxHPImage.rectTransform.sizeDelta;
        }
        private Vector2 getHPbarSize(int hp, int maxHP)
        {
            return new Vector2((float)(hp) / maxHP * hpBarSize.x, hpBarSize.y);
        }
        public void Show(Sprite player_icon, Sprite weapon_icon, string weapon_name, int maxHP, int curHP, int hit, int dmg, int crt, int dmgCount)
        {
            PlayerIcon.sprite = player_icon;
            WeaponIcon.sprite = weapon_icon;
            WeaponName.text = weapon_name;
            HP.text = curHP.ToString();
            HIT.text = "HIT    " + hit.ToString();
            DMG.text = "DMG   " + dmg.ToString();
            CRT.text = "CRT    " + crt.ToString();
            int afterHP = curHP - dmgCount * dmg;
            AfterDamageHPValue.text = afterHP.ToString();
            MaxHPImage.rectTransform.sizeDelta = getHPbarSize(maxHP, maxHP);
            CurHPImage.rectTransform.sizeDelta = getHPbarSize(curHP, maxHP);
            AfterDamageHPImage.rectTransform.sizeDelta = getHPbarSize(afterHP, maxHP);
            base.Show();
        }
    }
}