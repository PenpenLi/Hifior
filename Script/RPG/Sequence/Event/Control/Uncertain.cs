using UnityEngine;

namespace Sequence
{
    public class Uncertain : SequenceEvent
    {
        [Range(0, 100)]
        public int Propability;
        public GameObject WhenTrue;
        public GameObject WhenFalse;
        public override void OnEnter()
        {
            if (Random.Range(0, 100) <= Propability)
            {
                if (WhenFalse != null)
                    WhenFalse.SetActive(false);
                else
                {
                    //将不执行，同时不改变Enable
                    RootSequence.EventRef.Enable = true;
                    return;
                }
            }
            else
            {
                WhenTrue.SetActive(false);
            }
        }
    }
}