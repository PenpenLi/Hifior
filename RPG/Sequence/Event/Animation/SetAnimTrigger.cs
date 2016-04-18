using UnityEngine;
namespace Sequence
{
    public class SetAnimTrigger : SequenceEvent
    {
        [Tooltip("Reference to an Animator component in a game object")]
        public Animator animator;

        [Tooltip("Name of the trigger Animator parameter that will have its value changed")]
        public string parameterName;

        public override void OnEnter()
        {
            if (animator != null)
            {
                animator.SetTrigger(parameterName);
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (animator == null)
            {
                return "Error: No animator selected";
            }

            return animator.name + " (" + parameterName + ")";
        }
    }

}