using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CharacterBattleInfoElement : AbstractUI, IPointerClickHandler
    {
        private Color enable_color = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1.0f);
        private Color disable_color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1.0f);
        private Color forceYes_color = new Color(30f / 255f, 180f / 255f, 30f / 255f, 1.0f);
        private enum toBattle
        {
            ForceNo = -1,
            UserDefine = 0,
            ForceYes = 1
        }
        public int index;
        public bool bEnable;
        public int state;
        public Image icon;
        public Text charname;
        public Text job;
        public Text lv;
        public Text exp;
        public Text mhp;
        public void Init(int index, CharacterInfo ch, int state)//此处确定必须出场和不允许出场的人物，必须出场的放到最前面，不允许出场的强制放到最后面
        {
            this.index = index;
            this.state = state;
            PlayerDef def = ResourceManager.GetPlayerDef(ch.ID);
            icon.sprite = def.Portrait;
            charname.text = def.CommonProperty.Name;
            job.text = ch.Career.ToString();
            lv.text = ch.Level.ToString();
            exp.text = ch.Exp.ToString();
            mhp.text = ch.Attribute.HP.ToString();

            //初始状态由父类中的count决定 如果已经大于最大出场人物，则后面的人物都显示为灰的
            if (state == (int)toBattle.ForceNo)
            {
                bEnable = false;
                charname.color = disable_color;
            }
            if (state == (int)toBattle.ForceYes)
            {
                bEnable = true;
                SelectToBattlePanel.selectToBattle.currentSelectCount++;
                charname.color = forceYes_color;
            }

            if (state == (int)toBattle.UserDefine)
            {
                if (index >= SelectToBattlePanel.selectToBattle._limit)
                {
                    charname.color = disable_color;
                    bEnable = false;
                }
                else
                {
                    charname.color = enable_color;
                    bEnable = true;
                    SelectToBattlePanel.selectToBattle.currentSelectCount++;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)//如果是必须要出场的则显示为高亮绿色，且不接受事件
        {
            if (state != (int)toBattle.UserDefine)
                return;
            if (!bEnable && SelectToBattlePanel.selectToBattle.currentSelectCount >= SelectToBattlePanel.selectToBattle._limit)
                return;
            bEnable = !bEnable;
            if (bEnable)
            {
                charname.color = enable_color;
                SelectToBattlePanel.selectToBattle.currentSelectCount++;
            }
            else
            {
                charname.color = disable_color;
                SelectToBattlePanel.selectToBattle.currentSelectCount--;
            }
            SelectToBattlePanel.selectToBattle.RefreshSelectCount();
        }
    }
}