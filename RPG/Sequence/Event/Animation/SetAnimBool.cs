using UnityEngine;
namespace Sequence
{
	public class SetAnimBool : SequenceEvent 
	{
		[Tooltip("Reference to an Animator component in a game object")]
		public Animator animator;

		[Tooltip("Name of the boolean Animator parameter that will have its value changed")]
		public string parameterName;

		[Tooltip("The boolean value to set the parameter to")]
		public bool value;

		public override void OnEnter()
		{
			if (animator != null)
			{
				animator.SetBool(parameterName, value);
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