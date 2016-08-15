using UnityEngine;

namespace Sequence
{
    public class SetAnimInteger : SequenceEvent 
	{
		[Tooltip("Reference to an Animator component in a game object")]
		public Animator animator;

		[Tooltip("Name of the integer Animator parameter that will have its value changed")]
		public string parameterName;

		[Tooltip("The integer value to set the parameter to")]
		public int value;

		public override void OnEnter()
		{
			if (animator != null)
			{
				animator.SetInteger(parameterName, value);
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