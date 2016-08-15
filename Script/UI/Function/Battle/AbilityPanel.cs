using UnityEngine.UI;

namespace RPG.UI
{
    public class AbilityPanel : IPanel
    {
        public Text tSTR;
        public Image iSTR;
        public Text tWIS;
        public Image iWIS;
        public Text tDEX;
        public Image iDEX;
        public Text tAGI;
        public Image iAGI;

        public Text tLUK;
        public Image iLUK;
        public Text tDEF;
        public Image iDEF;
        public Text tRES;
        public Image iRES;

        public void Init(RPGCharacter ch)
        {
            tSTR.text = ch.GetPhysicalPower().ToString();
            iSTR.GetComponent<ValueBar>().initBar(ch.GetPhysicalPower(), ch.GetPhysicalPower(), 5);
            tWIS.text = ch.GetMagicalPower().ToString();
            iWIS.GetComponent<ValueBar>().initBar(ch.GetMagicalPower(), ch.GetMagicalPower(), 5);
            tDEX.text = ch.GetSkill().ToString();
            iDEX.GetComponent<ValueBar>().initBar(ch.GetSkill(), ch.GetSkill(), 5);
            tAGI.text = ch.GetSpeed().ToString();
            iAGI.GetComponent<ValueBar>().initBar(ch.GetSpeed(), ch.GetSpeed(), 5);
            tLUK.text = ch.GetLuck().ToString();
            iLUK.GetComponent<ValueBar>().initBar(ch.GetLuck(), ch.GetLuck(), 5);
            tDEF.text = ch.GetPhysicalDefense().ToString();
            iDEF.GetComponent<ValueBar>().initBar(ch.GetPhysicalDefense(), ch.GetPhysicalDefense(), 5);
            tRES.text = ch.GetMagicalDefense().ToString();
            iRES.GetComponent<ValueBar>().initBar(ch.GetMagicalDefense(), ch.GetMagicalDefense(), 5);
        }
    }
}