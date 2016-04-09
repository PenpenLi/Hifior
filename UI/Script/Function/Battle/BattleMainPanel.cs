using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.UI
{
    public class BattleMainPanel : IPanel
    {
        public Button environment;
        public Button army;
        public Button instruction;
        public Button interrupt;
        public Button endturn;
        [Header("结束按钮弹出窗口")]
        public Sprite EndModalPanelIcon;
        public Sprite EndModalPanelBackground;
        /// <summary>
        /// 结束本回合被点击
        /// </summary>
        public UnityEvent OnEndPlayerTurnClick;

        protected override void Awake()
        {
            base.Awake();

            environment.onClick.AddListener(Button_Environment);
            army.onClick.AddListener(Button_Army);
            instruction.onClick.AddListener(Button_Instruction);
            interrupt.onClick.AddListener(Button_Interrpt);
            endturn.onClick.AddListener(Button_EndTurn);
        }

        public override void Show()
        {
            base.Show();

            GetComponentInChildren<Button>().Select();
        }
        void Button_Environment()
        {
            UIController.Instance.GetUI<RPG.UI.ConfigPanel>().Show();
        }
        void Button_Army()
        {

        }
        void Button_Instruction()
        {

        }
        void Button_Interrpt()
        {

        }
        void Button_EndTurn()
        {
            base.Hide(false);

            ModalPanelDetail details = new ModalPanelDetail("是否结束本回合所有行动？", EndModalPanelIcon, EndModalPanelBackground,
                new EventButtonDetail("确认", EndTurnConfirm),
                new EventButtonDetail("取消", EndTurnCancel));
            UIController.Instance.GetUI<ModalPanel>().Show(details);
        }
        /// <summary>
        /// 确认结束回合
        /// </summary>
        private void EndTurnConfirm()
        {
            OnEndPlayerTurnClick.Invoke();
        }

        /// <summary>
        /// 确认结束回合
        /// </summary>
        private void EndTurnCancel()
        {
            Debug.Log("取消结束回合");
            this.Show();
        }
        public override void Tick(float DeltaTime)
        {
            base.Tick(DeltaTime);
            if (Input.GetButton("Cancel"))
            {
                base.Hide();
                Utils.GameUtil.DelayFunc(GetGameInstance(), () => GetPlayerPawn<Pawn_BattleArrow>().SetArrowActive(true), 0.1f);
            }
        }
    }
}