using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
namespace Sequence
{
    //[HierarchyIcon("TalkWithBackground.png", 2)]
    [AddComponentMenu("Sequence/Talk")]
    public class Talk : SequenceEvent
    {
        public string[] Code;
        public float fadeDuration = 1.0f;
        public override void OnEnter()
        {
            var TalkDialog = gameMode.UIManager.TalkDialog;
            Assert.IsNotNull(TalkDialog, "无法获取到 TalkWithBackground");
            TalkDialog.RegisterHideEvent(Continue);
            TalkDialog.Show(Code);
        }
        public override bool OnStopExecuting()
        {
            return true;
        }
    }

}
