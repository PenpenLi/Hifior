using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CharStateBiggerPanel : IPanel
    {
        public Text Text_Name;
        public Text Text_Job;
        public Text Text_LV;
        public Text Text_EXP;
        public Text Text_HP;
        public void Init(CharacterLogic ch)
        {
            Text_Name.text = ch.GetName();
            Text_Job.text = ch.GetCareerName();
            Text_LV.text = ch.Info.Level.ToString();
            Text_EXP.text = ch.GetExp().ToString();
            string curHp = null;
            if (ch.GetCurrentHP() == ch.Info.MaxHP)
                curHp = "<color=green>" + ch.GetCurrentHP() + "</color>";
            if (ch.GetCurrentHP() >= ch.Info.MaxHP / 2 && ch.GetCurrentHP() < ch.Info.MaxHP)
                curHp = "<color=orange>" + ch.GetCurrentHP() + "</color>";
            if (ch.GetCurrentHP() < ch.Info.MaxHP / 2)
                curHp = "<color=red>" + ch.GetCurrentHP() + "</color>";
            Text_HP.text =curHp + "/" + ch.GetMaxHP();
        }
    }
}