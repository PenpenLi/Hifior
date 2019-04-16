using UnityEngine;
using System.Collections;
public static class BaseClassExtendMethod
{
    public static GameMode gameMode;
    [RuntimeInitializeOnLoadMethod]
    public static void RunOnStart()
    {
        gameMode = GameMode.Instance;
    }
    public static void SetPositionX(this Transform tran, float x)
    {
        tran.position = new Vector3(x, tran.position.y, tran.position.z);
    }
    public static void SetPositionY(this Transform tran, float y)
    {
        tran.position = new Vector3(tran.position.x, y, tran.position.z);
    }
    public static void SetPositionZ(this Transform tran, float z)
    {
        tran.position = new Vector3(tran.position.x, tran.position.y, z);
    }
    public static void DelaySetActive(this GameObject gameObject, float delayTime, bool visible)
    {
        gameMode.StartCoroutine(IEnumDelayFunc(() => gameObject.SetActive(visible), delayTime));
    }
    public static void DelayDestroy(this GameObject tran, float delayTime)
    {
        Object.Destroy(tran, delayTime);
    }
    public static void DelaySetEnable(this MonoBehaviour behavior, float delayTime, bool enable)
    {
        gameMode.StartCoroutine(IEnumDelayFunc(() => behavior.enabled = enable, delayTime));
    }
    private static IEnumerator IEnumDelayFunc(UnityEngine.Events.UnityAction action, float duration)
    {
        yield return new WaitForSeconds(duration);
        action.Invoke();
    }

}
