using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CharacterElement : AbstractUI, IPointerClickHandler
    {
        public Text textName;
        public Image imageIcon;

        public int CharIndex;
        public RPGCharacter gameChar;

        public void init(int index, RPGCharacter ch)
        {
            CharIndex = index;
            gameChar = ch;
            textName.text = ch.GetCharacterName();
            imageIcon.sprite = ch.GetPortrait();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!TidyUpPanel.tidyUp.bWaitSelectSecond)
            {
                if (TidyUpPanel.tidyUp.selectedCharIndex != this.CharIndex)
                    TidyUpPanel.tidyUp.setSelectChar(this.gameObject, CharIndex);
                else
                {
                    TidyUpPanel.tidyUp.ShowCommandWindow();
                }
            }
            else
            {
                if (TidyUpPanel.tidyUp.selectedCharIndex == this.CharIndex)//不可选择第二个与第一个一样
                    return;
                TidyUpPanel.tidyUp.setSelectExchangeChar(this.gameObject, CharIndex);
            }
        }
    }
}
