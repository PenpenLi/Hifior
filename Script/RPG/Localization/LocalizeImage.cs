using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class LocalizeImage : MonoBehaviour, IModifyLanguageHandle
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


    /// <summary>
    /// This function is called by the Localization manager via a broadcast SendMessage.
    /// </summary>

    public void OnLocalize()
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (!string.IsNullOrEmpty(key))
            {
                Image lbl = GetComponent<Image>();

                if (lbl != null)
                {
                    string resPath = "LocalizedImage/" + Localization.language + "/0";
                    Sprite sp = Resources.Load<Sprite>(resPath);
                    UnityEngine.Assertions.Assert.IsNotNull(sp, resPath + "无法正确加载LocalizeImage资源");
                    if (sp != null)
                        lbl.sprite = sp;
                }
            }
        }
    }
}
