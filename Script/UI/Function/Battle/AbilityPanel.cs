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

        public void Init(CharacterLogic ch)
        {
            var att = ch.Info.Attribute;
            var max = ResourceManager.GetCareerDef( ch.GetCareer()).MaxAttribute;
            tSTR.text = att.PhysicalPower.ToString();
            iSTR.GetComponent<ValueBar>().initBar(max.PhysicalPower, att.PhysicalPower, 5);
            tWIS.text =att.MagicalPower.ToString();
            iWIS.GetComponent<ValueBar>().initBar(max.MagicalPower, att.MagicalPower, 5);
            tDEX.text = att.Skill.ToString();
            iDEX.GetComponent<ValueBar>().initBar(max.Skill, att.Skill, 5);
            tAGI.text =att.Speed.ToString();
            iAGI.GetComponent<ValueBar>().initBar(max.Speed, att.Speed, 5);
            tLUK.text =att.Luck.ToString();
            iLUK.GetComponent<ValueBar>().initBar(max.Luck, att.Luck, 5);
            tDEF.text =att.PhysicalDefense.ToString();
            iDEF.GetComponent<ValueBar>().initBar(max.PhysicalDefense, att.PhysicalDefense, 5);
            tRES.text =att.MagicalDefense.ToString();
            iRES.GetComponent<ValueBar>().initBar(max.MagicalDefense, att.MagicalDefense, 5);
        }
    }
}