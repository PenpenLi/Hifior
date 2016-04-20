using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.UI;
//写入加载和接触显示时的效果，所有的UI对象均继承此类，并且此类含一个全局变量用于指示当前激活可操作状态和激活了显示状态的UI标志
public class UIController : MonoBehaviour
{
    private static UIController instance;
    private static ScreenMask m_screenMask;

    public static ScreenMask ScreenFade
    {
        get
        {
            if (m_screenMask == null)
                m_screenMask = GetMonoScript<ScreenMask>(m_screenMask, "Prefab/UIElement/BlackFadeMask");
            return m_screenMask;
        }
    }

    public List<IPanel> UIList;

    public T GetUI<T>() where T : IPanel
    {
        foreach (IPanel p in UIList)
        {
            if (p is T)
                return p as T;
        }
        Debug.LogError("没有找到该UI:" + typeof(T).ToString());
        return null;
    }

    public static UIController Instance
    {
        get
        {
            if (!instance)
            {
                GameObject obj = GameObject.Find("Canvas");
                if (obj)
                {
                    instance = Utils.MiscUtil.GetComponentNotNull<UIController>(obj);
                }
                else
                {
                    instance = Utils.MiscUtil.GetComponentNotNull<UIController>(Instantiate<GameObject>(Resources.Load<GameObject>(ConstTable.PREFAB_CANVAS)));
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    private static T GetMonoScript<T>(T MonoScript, string PrefabPath) where T : MonoBehaviour
    {
        if (!MonoScript)
        {
            MonoScript = Utils.MiscUtil.GetComponentNotNull<T>(Instantiate<GameObject>(Resources.Load<GameObject>(PrefabPath)));
            MonoScript.gameObject.SetActive(true);
            MonoScript.transform.SetParent(Instance.transform);
        }
        return MonoScript;
    }
    public static void ScreenDarkToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        ScreenFade.Show(false, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenFade.GetComponent<ScreenMask>().Hide(); }, duration);
    }

    public static void ScreenNormalToDark(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        ScreenFade.Show(true, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenFade.GetComponent<ScreenMask>().Hide(); }, duration);
    }
    public static void ScreenWhiteToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        ScreenFade.Show(false, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenFade.GetComponent<ScreenMask>().Hide(); }, duration);
    }
    public static void ScreenNormalToWhite(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        ScreenFade.Show(true, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenFade.GetComponent<ScreenMask>().Hide(); }, duration);
    }
}
