using UnityEngine;
namespace Sequence
{
    public class SetAnimFloat : SequenceEvent
    {
        [Tooltip("Reference to an Animator component in a game object")]
        public Animator animator;

        [Tooltip("Name of the float Animator parameter that will have its value changed")]
        public string parameterName;

        [Tooltip("The float value to set the parameter to")]
        public float value;

        public override void OnEnter()
        {
            if (animator != null)
            {
                animator.SetFloat(parameterName, value);
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