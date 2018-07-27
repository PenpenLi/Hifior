using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Clear Stage")]
    public class ClearStage : SequenceEvent
    {
        public override void OnEnter()
        {
           // GetGameMode<GM_Battle>().ClearTheStage();
        }
    }
}