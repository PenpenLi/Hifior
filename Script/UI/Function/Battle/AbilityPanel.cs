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
            var att = ch.Logic().Info.Attribute;
            tSTR.text = att.PhysicalPower.ToString();
            iSTR.GetComponent<ValueBar>().initBar(att.PhysicalPower, att.PhysicalPower, 5);
            tWIS.text =att.MagicalPower.ToString();
            iWIS.GetComponent<ValueBar>().initBar(att.MagicalPower, att.MagicalPower, 5);
            tDEX.text = att.Skill.ToString();
            iDEX.GetComponent<ValueBar>().initBar(att.Skill, att.Skill, 5);
            tAGI.text =att.Speed.ToString();
            iAGI.GetComponent<ValueBar>().initBar(att.Speed, att.Speed, 5);
            tLUK.text =att.Luck.ToString();
            iLUK.GetComponent<ValueBar>().initBar(att.Luck, att.Luck, 5);
            tDEF.text =att.PhysicalDefense.ToString();
            iDEF.GetComponent<ValueBar>().initBar(att.PhysicalDefense, att.PhysicalDefense, 5);
            tRES.text =att.MagicalDefense.ToString();
            iRES.GetComponent<ValueBar>().initBar(att.MagicalDefense, att.MagicalDefense, 5);
        }
    }
}