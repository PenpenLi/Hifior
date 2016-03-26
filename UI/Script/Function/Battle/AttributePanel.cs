using UnityEngine;

namespace RPG.UI
{
    public class AttributePanel : IPanel
    {
        public GameObject charStateBiggerPanel;
        public GameObject attackInfoPanel;
        public GameObject abilityPanel;
        public GameObject itemPanel;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(RPGCharacter ch)
        {
            if (gameObject.activeSelf) return;
            this.gameObject.SetActive(true);
            charStateBiggerPanel.GetComponent<CharStateBiggerPanel>().init(ch);
            attackInfoPanel.GetComponent<AttackInfoPanel>().init(ch);
            abilityPanel.GetComponent<AbilityPanel>().init(ch);
            itemPanel.GetComponent<ItemPanel>().init(ch);
        }

        void Update()
        {
            if (Input.GetButton("Cancel") && gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }
}