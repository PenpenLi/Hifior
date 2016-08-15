using UnityEngine;

namespace RPG.UI
{
    public class ClearStagePanel : IPanel
    {
        public override void Show()
        {
            base.Show();
            UIController.ScreenNormalToDark(2.0f, false, Hide);
        }
    }
}
