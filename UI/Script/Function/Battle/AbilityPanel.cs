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
        // Use this for initialization

        public void init(RPGCharacter ch)
        {
           /* tSTR.text = ch.attribute.STR.ToString();
            iSTR.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[1], ch.attribute.STR, ch.attribute.ExtraAttribute.Extra_str);
            tWIS.text = ch.attribute.WIS.ToString();
            iWIS.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[2], ch.attribute.WIS, ch.attribute.ExtraAttribute.Extra_wis);
            tDEX.text = ch.attribute.DEX.ToString();
            iDEX.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[3], ch.attribute.DEX, ch.attribute.ExtraAttribute.Extra_dex);
            tAGI.text = ch.attribute.AGI.ToString();
            iAGI.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[4], ch.attribute.AGI, ch.attribute.ExtraAttribute.Extra_agi);
            tLUK.text = ch.attribute.LUK.ToString();
            iLUK.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[7], ch.attribute.LUK, ch.attribute.ExtraAttribute.Extra_luk);
            tDEF.text = ch.attribute.DEF.ToString();
            iDEF.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[5], ch.attribute.DEF, ch.attribute.ExtraAttribute.Extra_def);
            tRES.text = ch.attribute.RES.ToString();
            iRES.GetComponent<ValueBar>().initBar(Table._JobTable.getAbilityLimits(ch.attribute.Job)[6], ch.attribute.RES, ch.attribute.ExtraAttribute.Extra_res);*/
        }
    }
}