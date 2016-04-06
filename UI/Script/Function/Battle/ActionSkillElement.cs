using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class ActionSkillElement : IPanel, IPointerClickHandler, IPointerEnterHandler
    {

        public int Index;
        public Text SkillName;
        public int Key;
        public string Descrpition;

        public string Name
        {
            get
            {
                return SkillName.text;
            }
        }
        public void init(int Index, int Key)
        {
            this.Index = Index;
            this.Key = Key;
            this.SkillName.text = ResourceManager.GetPassiveSkillDef(Key).CommonProperty.Name;
            this.Descrpition = ResourceManager.GetPassiveSkillDef(Key).CommonProperty.Description;
            if (Index == 0)
                UIController.Instance.GetUI<RPG.UI.SkillMenu> ().SetDescription(Descrpition);
        }
        public void OnPointerClick(PointerEventData eventData)//点击切换状态并执行行为
        {
            //UIController.Instance.GetUI<RPG.UI.ActionMenu>().Show(1, SkillName.text);
            /*SLGLevel.SLG._slgSkill.CommandSkill(key, SLGLevel.SLG.getCurrentSelectGameChar().TileCoords);
            SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SKILLSELECT));*/
        }

        public void OnPointerEnter(PointerEventData eventData)//鼠标进入则显示技能的介绍
        {
            UIController.Instance.GetUI<RPG.UI.SkillMenu>().SetDescription(Descrpition);
        }
    }
}