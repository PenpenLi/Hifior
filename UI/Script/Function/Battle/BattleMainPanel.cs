using UnityEngine;

namespace RPG.UI
{
    public class BattleMainPanel : IPanel
    {
        public UnityEngine.UI.Button environment;
        public UnityEngine.UI.Button army;
        public UnityEngine.UI.Button instruction;
        public UnityEngine.UI.Button interrupt;
        public UnityEngine.UI.Button endturn;

        protected override  void Awake()
        {
            base.Awake();

            environment.onClick.AddListener(Button_Environment);
            army.onClick.AddListener(Button_Army);
            instruction.onClick.AddListener(Button_Instruction);
            interrupt.onClick.AddListener(Button_Interrpt);
            endturn.onClick.AddListener(Button_EndTurn);
            Hide();
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
            /*Widget.WidgetYesNo.Instance.InitCallBack(delegate
            {
                foreach (RPGCharacter ch in SLGLevel.SLG._GameCharList.char_player)
                {
                    if (ch.EnableAction)
                    {
                        SLGLevel.SLG.setCurrentSelectGameChar(ch);
                        SLGLevel.SLG.FinishAction();
                    }
                }
            });*/
            Widget.WidgetYesNo.Instance.Show("是否结束本回合所有行动？");
        }
    }
}