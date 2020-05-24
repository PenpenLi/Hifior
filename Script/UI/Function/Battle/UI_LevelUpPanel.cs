using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPG.UI
{
    public class UI_LevelUpPanel : IPanel
    {
        public GameObject PanelJobAndLevel;
        public Text[] tAdd;
        public Text[] tAbilityValue;
        private bool bShowFinish;

        protected override void Awake()
        {
            base.Awake();

            resetAllAddText();
            Hide();
            PanelJobAndLevel.SetActive(false);
        }
        void Update()
        {
            if (gameMode.InputManager.GetYesInput() && bShowFinish)
                Hide(false);
        }
        /// <summary>
        /// int 数组,Level,HP,PhysicalPower,MagicalPower,Skill,Speed,Intel,PhysicalDefense,MagicalDefense
        /// </summary>
        /// <param name="chName"></param>
        /// <param name="level"></param>
        /// <param name="original"></param>
        /// <param name="add"></param>
        public void Show(string chName, int[] original, int[] add) //add为增长的8个数值
        {
            bShowFinish = false;
            gameObject.SetActive(true);
            PanelJobAndLevel.SetActive(true);
            for (int i = 0; i < tAbilityValue.Length; i++)
            {
                tAbilityValue[0].text = original[i].ToString();
            }

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
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i <= 8; i++)
            {
                if (tAdd[i].text == "")//当前文本为空
                {
                    yield return null;
                }
                else
                {
                    tAdd[i].gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.3f);
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