using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

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
        private UnityEvent OnEndPlayerTurnClick;

        protected override void Awake()
        {
            base.Awake();
            
            army.onClick.AddListener(Button_Army);
            instruction.onClick.AddListener(Button_Instruction);
            interrupt.onClick.AddListener(Button_Interrpt);
            endturn.onClick.AddListener(Button_EndTurn);
        }
        public override void BeginPlay()
        {
            base.BeginPlay();
            OnEndPlayerTurnClick = new UnityEvent();
            OnEndPlayerTurnClick.AddListener(GetPlayerPawn<Pawn_BattleArrow>().EndPlayerTurn);
        }
        public override void Show()
        {
            base.Show();

            GetComponentInChildren<Button>().Select();
        }
        void Button_Environment()
        {
            //UIController.Instance.GetUI<RPG.UI.ConfigPanel>().Show();
            ChapterRecordCollection ChapterRecord = new ChapterRecordCollection();
            ChapterRecord.Chapter = 1;
            List<int> AvailablePlayer = new List<int>();
            AvailablePlayer.Add(1);
            AvailablePlayer.Add(2);
            ChapterRecord.AvailablePlayers = AvailablePlayer;
            ChapterRecord.Ware.Money = 10000;
            ChapterRecord.RefreshPlayersInfo(GetGameStatus<UGameStatus>().GetLocalPlayers());
            ChapterRecord.SaveBinary();
        }
        void Button_Army()
        {
            ChapterRecordCollection ChapterRecord = new ChapterRecordCollection();
            if (ChapterRecord.Exists())
            {
                ChapterRecord = ChapterRecord.LoadBinary<ChapterRecordCollection>();

                Debug.Log(ChapterRecord.Ware.Money);
                Debug.Log(ChapterRecord.PlayersInfo);
            }
            else
            {
                Debug.LogError("请先保存" + ChapterRecord.GetFullRecordPathName());
            }
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
        public override void OnCancelKeyDown()
        {
            base.Hide();
            Utils.GameUtil.DelayFunc(this, () => GetPlayerPawn<Pawn_BattleArrow>().SetArrowActive(true), 0.1f);
        }
    }
}