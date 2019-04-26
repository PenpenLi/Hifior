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
        public Text tINT;
        public Image iINT;
        public Text tDEF;
        public Image iDEF;
        public Text tRES;
        public Image iRES;
        public Text tBDY;
        public Image iBDY;
        public Text tMOV;
        public Image iMOV;

        public void Init(CharacterLogic ch)
        {
            var att = ch.Info.Attribute;
            var max = ResourceManager.GetCareerDef( ch.GetCareer()).MaxAttribute;
            var len = CharacterAttribute.Max;
            tSTR.text = att.PhysicalPower.ToString();
            iSTR.GetComponent<ValueBar>().initBar(len.PhysicalPower, max.PhysicalPower, att.PhysicalPower, 5);
            tWIS.text =att.MagicalPower.ToString();
            iWIS.GetComponent<ValueBar>().initBar(len.MagicalPower, max.MagicalPower, att.MagicalPower, 5);
            tDEX.text = att.Skill.ToString();
            iDEX.GetComponent<ValueBar>().initBar(len.Skill, max.Skill, att.Skill, 5);
            tAGI.text =att.Speed.ToString();
            iAGI.GetComponent<ValueBar>().initBar(len.Speed, max.Speed, att.Speed, 5);
            tINT.text =att.Intel.ToString();
            iINT.GetComponent<ValueBar>().initBar(len.Intel, max.Intel, att.Intel, 5);
            tDEF.text =att.PhysicalDefense.ToString();
            iDEF.GetComponent<ValueBar>().initBar(len.PhysicalDefense, max.PhysicalDefense, att.PhysicalDefense, 5);
            tRES.text =att.MagicalDefense.ToString();
            iRES.GetComponent<ValueBar>().initBar(len.MagicalDefense, max.MagicalDefense, att.MagicalDefense, 5);
            tBDY.text = att.BodySize.ToString();
            iBDY.GetComponent<ValueBar>().initBar(len.BodySize, max.BodySize, att.BodySize, 0);
            tMOV.text = att.Movement.ToString();
            iMOV.GetComponent<ValueBar>().initBar(len.Movement, max.Movement, att.Movement, 0);
        }
    }
}