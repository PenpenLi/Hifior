using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class PrepareCommandMenu : IPanel
    {
        public BattleReadyPanel Main;
        public SelectToBattlePanel SelectPlayerPanel;
        [Header("按钮选择")]
        public Button CharacterSelect;
        public Button SettingConfirm;
        public Button TidyUp;
        public Button StatusConfirm;
        public Button EnvironmentSetting;
        public Button Save;
        public Button StartBattle;

        [Header("开始战斗弹出确认界面的图片")]
        public Sprite ModalIcon_StartBattle;
        public Sprite ModalBG_StartBattle;

        protected override void Awake()
        {
            base.Awake();
            StartBattle.onClick.AddListener(Button_StartBattle);
            CharacterSelect.onClick.AddListener(Button_SelectCharacter);
        }
        /// <summary>
        /// 选择出场人物
        /// </summary>
        public void Button_SelectCharacter()
        {
            SelectPlayerPanel.Show();
        }
        /// <summary>
        /// 开始战斗
        /// </summary>
        public void Button_StartBattle()
        {
            ModalPanelDetail details = new ModalPanelDetail("是否开始进行战斗？", ModalIcon_StartBattle, ModalBG_StartBattle, new EventButtonDetail("确认", StartBattleConfirm), new EventButtonDetail("取消", StartBattleCancel));
            UIController.Instance.GetUI<ModalPanel>().Show(details);
        }
        public void StartBattleConfirm()
        {
            UIController.Instance.GetUI<ModalPanel>().Hide();
            UIController.Instance.GetUI<BattleReadyPanel>().Hide();
        }
        public void StartBattleCancel()
        {
            UIController.Instance.GetUI<ModalPanel>().Hide();
        }
    }
}