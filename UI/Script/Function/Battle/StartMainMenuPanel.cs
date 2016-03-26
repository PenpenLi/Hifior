using UnityEngine;
//UI界面只做和UI相关的事物，不处理例如音乐等无光的事物

namespace RPG.UI
{
    public class StartMainMenuPanel : IPanel
    {
        public UnityEngine.UI.Button[] buttons;
        private SaveAndLoadPanel _save_load;
        private const int Button_Continue = 0;
        private const int Button_StartNewGame = 1;
        private const int Button_Extra = 2;
        /*
        void Awake()
        {
            SManage.Transition(new SMInput(BATTLE_STATE.MainMenu));

            _save_load = UISet.Panel_SaveAndLoad;
            _save_load.init(false, true);//读取存档初始化
            if (_save_load.isHaveSave())
                buttons[Button_Continue].gameObject.SetActive(true);
            else
            {
                buttons[Button_Continue].gameObject.SetActive(false);
            }
        }
        void Start()
        {
            Util.GameUtils.ScreenDarkToNormal(2.0f);
            //Util.GameUtils.UIColorFade(gameObject, 2.0f, Color.black, Color.white);
        }
        public void OnContinueClick()
        {
            _save_load.init(false, true);//读取初始化
            _save_load.LastState = 0;//是主菜单状态
            ToSelectLoad();
        }
        public void OnStartNewGameClick()
        {
            _save_load.init(true, true);//存储初始化
            _save_load.bContinueFromSave = true;
            _save_load.LastState = 0;//是主菜单状态
            ToSelectLoad();
        }
        private void ToSelectLoad()
        {
            SManage.Transition(new SMInput(BATTLE_STATE.SelectLoad));
            Hide();
        }
        public void OnExtraClick()
        {

        }
        public void OnExitGameClick()
        {
            Util.GameUtils.Quit();
        }*/
    }
}