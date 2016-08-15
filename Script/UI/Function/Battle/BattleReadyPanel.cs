using UnityEngine;

namespace RPG.UI
{
    public class BattleReadyPanel : IPanel
    {
        [Header("选定面板")]
        public PrepareCommandMenu CommandPanel;
        public TidyUpPanel TidyUpPanel;
        public SelectToBattlePanel SelectCharToBattlePanel;
        public BattleStatusConfirmPanel StateConfirmPanel;
        public ConfigPanel ConfigPanel;

        protected override void Awake()
        {
            base.Awake();
            

            /*ConfirmStartPanel.transform.FindChild("Button_YES").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnConfirmStartButton_Yes);
            ConfirmStartPanel.transform.FindChild("Button_NO").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnConfirmStartButton_No);

            TidyUpPanel.SetActive(false);
            SelectCharToBattlePanel.SetActive(false);
            StateConfirmPanel.SetActive(false);
            ConfirmStartPanel.SetActive(false);*/
            Hide();
        }
        public override void Show()
        {
            base.Show();

        }
        #region CommandPanel
        public void OnCommandButton_CharSelect()//选择出场人物
        {
            //SelectCharToBattlePanel.SetActive(true);
        }

        public void OnCommandButton_SettingConfirm()//设置人物的位置
        {
            //待处理
        }

        public void OnCommandButton_TidyUp()//物品整理
        {
            //TidyUpPanel.SetActive(true);
        }

        public void OnCommandButton_StateConfirm()//状态确认
        {
            //StateConfirmPanel.SetActive(true);
        }

        public void OnCommandButton_EnvironmentSetting()//环境设置Config
        {
            //ConfigPanel.SetActive(true);
        }

        public void OnCommandButton_Save()//保存游戏界面
        {
            //UISet.Panel_SaveAndLoad.init(true, false);
        }

        public void OnCommandButton_BattleStart()//开始游戏
        {
            //ConfirmStartPanel.SetActive(true);
        }
        #endregion
        #region ConfirmStartPanel EventListener
        public void OnConfirmStartButton_Yes()//Panel_ConfirmStart点击“是”
        {
            Hide();
           // ConfirmStartPanel.SetActive(false);
        }
        public void OnConfirmStartButton_No()
        {
          //  ConfirmStartPanel.SetActive(false);
        }
        #endregion
    }
}