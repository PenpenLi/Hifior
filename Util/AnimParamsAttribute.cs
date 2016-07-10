using UnityEngine;
/*
Use the AnimParams attribute on a string property/variable and the inspector will show a popup that lists all the parameter names available in the given Animator.
Pass the name of the Animator property as a string to the AnimParams attribute.

public class ExampleView : MonoBehaviour
{
    public Animator anim;

    // These are strings that we'll use to fire parameters in code
    // pass the name of the Animator variable/property as a string
    [AnimParams("anim")]
    public string paramIdleBool;

    [AnimParams("anim")]
    public string paramAttackTrigger;

    void Start()
    {
        // Use the property which will be filled with one of the
        // parameters names available in the animator in the inspector
        anim.SetBool(paramIdleBool, true);
    }
}
*/
namespace Utils
{
    public class AnimParamsAttribute : PropertyAttribute
    {
        public string animatorProp;

        public AnimParamsAttribute(string animatorPropName)
        {
            animatorProp = animatorPropName;
        }
    }
}
