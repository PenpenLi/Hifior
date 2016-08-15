using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Toggle Script Enable")]
    public class SetScriptActive : SequenceEvent
    {
        public MonoBehaviour Script;
        public bool Active;
        public override void OnEnter()
        {
            if (Script)
            {
                Script.enabled = Active;
            }
            Continue();
        }
        public override string GetSummary()
        {
            string s = Active ? "激活" : "非激活";
            return "设置脚本" + Script.name + "为" + s + "状态";
        }
    }
}
