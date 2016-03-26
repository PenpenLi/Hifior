using UnityEngine;

namespace RPG.UI
{
    public class BattleReadyPanel : IPanel
    {

        public GameObject CommandPanel;
        public GameObject TidyUpPanel;
        public GameObject SelectCharToBattlePanel;
        public GameObject StateConfirmPanel;
        public GameObject ConfirmStartPanel;
        public GameObject ConfigPanel;
        protected override void Awake()
        {
            base.Awake();

            CommandPanel.transform.FindChild("Button_CharSelect").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_CharSelect);
            CommandPanel.transform.FindChild("Button_SettingConfirm").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_SettingConfirm);
            CommandPanel.transform.FindChild("Button_TidyUp").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_TidyUp);
            CommandPanel.transform.FindChild("Button_StateConfirm").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_StateConfirm);
            CommandPanel.transform.FindChild("Button_EnvironmentSetting").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_EnvironmentSetting);
            CommandPanel.transform.FindChild("Button_Save").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_Save);
            CommandPanel.transform.FindChild("Button_BattleStart").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCommandButton_BattleStart);

            ConfirmStartPanel.transform.FindChild("Button_YES").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnConfirmStartButton_Yes);
            ConfirmStartPanel.transform.FindChild("Button_NO").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnConfirmStartButton_No);

            TidyUpPanel.SetActive(false);
            SelectCharToBattlePanel.SetActive(false);
            StateConfirmPanel.SetActive(false);
            ConfirmStartPanel.SetActive(false);
            Hide();
        }
        public bool Visible
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        #region CommandPanel
        public void OnCommandButton_CharSelect()//选择出场人物
        {
            SelectCharToBattlePanel.SetActive(true);
        }

        public void OnCommandButton_SettingConfirm()//设置人物的位置
        {
            //待处理
        }

        public void OnCommandButton_TidyUp()//物品整理
        {
            TidyUpPanel.SetActive(true);
        }

        public void OnCommandButton_StateConfirm()//状态确认
        {
            StateConfirmPanel.SetActive(true);
        }

        public void OnCommandButton_EnvironmentSetting()//环境设置Config
        {
            ConfigPanel.SetActive(true);
        }

        public void OnCommandButton_Save()//保存游戏界面
        {
            //UISet.Panel_SaveAndLoad.init(true, false);
        }

        public void OnCommandButton_BattleStart()//开始游戏
        {
            ConfirmStartPanel.SetActive(true);
        }
        #endregion
        #region ConfirmStartPanel EventListener
        public void OnConfirmStartButton_Yes()//Panel_ConfirmStart点击“是”
        {
            Hide();
            ConfirmStartPanel.SetActive(false);
        }
        public void OnConfirmStartButton_No()
        {
            ConfirmStartPanel.SetActive(false);
        }
        #endregion
    }
}