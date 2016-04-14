﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPG.UI
{
    public class LevelUpPanel : IPanel
    {
        public GameObject PanelJobAndLevel;
        public Text[] tAdd;
        public Text[] tAbilityValue;
        private bool bShowFinish;

        protected override void Awake()
        {
            base.Awake();

            resetAllAddText();
            gameObject.SetActive(false);
            PanelJobAndLevel.SetActive(false);
        }
        void Update()
        {
            if (Input.GetMouseButton(0) && bShowFinish)
                this.gameObject.SetActive(false);
        }
        public void Show(RPGCharacter ch, int[] add) //add为增长的8个数值
        {
            bShowFinish = false;
            gameObject.SetActive(true);
            PanelJobAndLevel.SetActive(true);

            tAbilityValue[0].text = ch.GetLevel().ToString();
            tAbilityValue[1].text = ch.GetMaxHP().ToString();
            tAbilityValue[2].text = ch.GetPhysicalPower().ToString();
            tAbilityValue[3].text = ch.GetMagicalPower().ToString();
            tAbilityValue[4].text = ch.GetSkill().ToString();
            tAbilityValue[5].text = ch.GetSpeed().ToString();
            tAbilityValue[6].text = ch.GetLuck().ToString();
            tAbilityValue[7].text = ch.GetPhysicalDefense().ToString();
            tAbilityValue[8].text = ch.GetMagicalDefense().ToString();

            for (int i = 0; i < 8; i++)
            {
                if (add[i] > 0)
                {
                    tAdd[i + 1].text = "+" + add[i].ToString();
                }
                else
                {
                    tAdd[i + 1].text = "";
                }
            }
            HideAllAddText();
            //设置完所有显示的内容
            StartCoroutine(animPlay());
        }
        IEnumerator animPlay()
        {
            int i = 0;
            tAdd[i].gameObject.SetActive(true);//必定显示
            while (true)
            {
                if (!tAdd[i].GetComponent<Animation>().isPlaying)//这个lv+1动画播放完毕
                {
                    i++;
                    if (i > 8)
                        break;
                    if (tAdd[i].text == "")//当前文本为空
                    {
                        yield return null;
                    }
                    else
                    {
                        tAdd[i].gameObject.SetActive(true);
                    }
                }
                yield return null;
            }
            bShowFinish = true;
        }
        void HideAllAddText()
        {
            foreach (Text t in tAdd)
                t.gameObject.SetActive(false);
        }
        void resetAllAddText()
        {
            foreach (Text t in tAdd)
                t.text = "";
            tAdd[0].text = "+1";
        }
    }
}