﻿using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class LocalizeText : MonoBehaviour, IModifyLanguageHandle
{/// <summary>
 /// Localization key.
 /// </summary>

    public string key;

    /// <summary>
    /// Manually change the value of whatever the localization component is attached to.
    /// </summary>

    public string value
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                Text lbl = GetComponent<Text>();

                if (lbl != null)
                {
                    lbl.text = value;
                }
            }
        }
    }

    bool mStarted = false;

    /// <summary>
    /// Localize the widget on enable, but only if it has been started already.
    /// </summary>

    void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        if (mStarted) OnLocalize();
    }

    /// <summary>
    /// Localize the widget on start.
    /// </summary>

    void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        mStarted = true;
        OnLocalize();
    }

    public void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 30), "English"))
        {
            Localization.language = "English";
        }
        if (GUI.Button(new Rect(0, 40, 100, 30), "French"))
        {
            Localization.language = "French";
        }
    }

    /// <summary>
    /// This function is called by the Localization manager via a broadcast SendMessage.
    /// </summary>

    public void OnLocalize()
    {
        if (string.IsNullOrEmpty(key))
        {
            Text lbl = GetComponent<Text>();
            if (lbl != null) key = lbl.text;
        }

        if (!string.IsNullOrEmpty(key)) value = Localization.Get(key);
    }
}
