using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
namespace RPG.UI
{
    public class UI_StartMenu : IPanel
    {
        public enum EMainMenuType
        {
            Continue,
            ButtonStartGame,
            Setting,
            Button_Extra,
            Button_ExitGame,
        }
        public Transform MenuRoot;
        public AudioClip Theme;
        private Button[] Menus;
        public override void BeginPlay()
        {
            base.Awake();
            SoundManage.Instance.PlayClip(Theme);
            Menus = GetComponentsInChildren<Button>();

            var startNewGame = GetButton(EMainMenuType.ButtonStartGame);
            {
                startNewGame.onClick.AddListener(gameMode.UIManager.RecordChapter.Show_NewGame);
            }
            var continueGame = GetButton(EMainMenuType.Continue);
            {
                continueGame.onClick.AddListener(gameMode.UIManager.RecordChapter.Show_Continue);
            }
            var exitGame = GetButton(EMainMenuType.Button_ExitGame);
            {
#if UNITY_EDITOR
                exitGame.onClick.AddListener(UnityEditor.EditorApplication.ExitPlaymode);
#endif
                exitGame.onClick.AddListener(Application.Quit);
            }
        }
        private Button GetButton(EMainMenuType type)
        {
            return Menus[(int)type];
        }

    }
}