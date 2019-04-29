using UnityEngine;

namespace RPG.UI
{
    public class UI_CharacterInfoPanel : IPanel
    {
        public GameObject charStateBiggerPanel;
        public GameObject abilityPanel;
        public GameObject itemPanel;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(CharacterLogic ch)
        {
            gameObject.SetActive(true);
            charStateBiggerPanel.GetComponent<CharStateBiggerPanel>().Init(ch);
            abilityPanel.GetComponent<AbilityPanel>().Init(ch);
            itemPanel.GetComponent<ItemPanel>().Init(ch);
        }
        public override void OnCancelKeyDown()
        {
        }
    }
}