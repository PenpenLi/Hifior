using System.Collections;
using UnityEngine;

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
public class SimpleInvoke
{
    public static IEnumerator DelayToInvokeDo(UnityEngine.Events.UnityAction action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        if (action != null)
            action();
    }
}