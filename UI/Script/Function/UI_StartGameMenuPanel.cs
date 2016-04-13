using UnityEngine.UI;
using System.Collections;
namespace RPG.UI
{
    public class UI_StartGameMenuPanel : IPanel
    {
        public Button StartGame;
        public RecordChapterPanel RecordChapter;

        public override void BeginPlay()
        {
            base.BeginPlay();

            StartGame.Select();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            StartGame.Select();
        }
        /// <summary>
        /// 点击开始游戏按钮
        /// </summary>
        public void Button_StartGame()
        {
            Hide();
            RecordChapter.ShowFadeAlpha();
        }
    }
}