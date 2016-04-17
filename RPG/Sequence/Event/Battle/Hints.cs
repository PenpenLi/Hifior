using UnityEngine;
using UnityEngine.Assertions;
namespace Sequence
{
    [AddComponentMenu("Sequence/Hints")]
    public class Hints : SequenceEvent
    {
        public string Caption = "提示";
        public string Content;

        public override void OnEnter()
        {
            RPG.UI.HintsPanel p = UIController.Instance.GetUI<RPG.UI.HintsPanel>();
            Assert.IsNotNull(p, "请将 HintsPanel 加入到 UIController里面");
            p.RegisterHideEvent(Continue);
            p.Show(Caption, Content);
        }
    }
}