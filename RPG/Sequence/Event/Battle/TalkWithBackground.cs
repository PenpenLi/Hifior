using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
namespace Sequence
{
    [HierarchyIcon("TalkWithBackground.png", 2)]
    [AddComponentMenu("Sequence/Talk(Background)")]
    public class TalkWithBackground : SequenceEvent
    {
        public int Background;
        public List<string> Code;
        public float fadeDuration = 1.0f;
        public override void OnEnter()
        {
            UIController.ScreenNormalToDark(fadeDuration, false, ShowTalkUI);
        }
        void ShowTalkUI()
        {
            RPG.UI.TalkWithBackground Talk = UIController.Instance.GetUI<RPG.UI.TalkWithBackground>();
            Assert.IsNotNull(Talk, "无法获取到 TalkWithBackground");
            Talk.RegisterHideEvent(OnHideTalkUI);
            Talk.Show(Code,Background);

            UIController.ScreenDarkToNormal(fadeDuration);
        }
        void OnHideTalkUI()
        {
            UIController.ScreenDarkToNormal(fadeDuration, Continue);
        }
    }

}
