using UnityEngine.UI;
using System.Collections;
namespace RPG.UI
{
    public class UI_StartGameMenuPanel : IPanel
    {
        public Button StartGame;

        public override void BeginPlay()
        {
            base.BeginPlay();

            StartGame.Select();
        }

        public void OnEnable()
        {
            StartGame.Select();
        }
    }
}